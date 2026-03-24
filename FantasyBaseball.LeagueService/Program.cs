using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FantasyBaseball.LeagueService.Database;
using FantasyBaseball.LeagueService.Database.Repositories;
using FantasyBaseball.LeagueService.Exceptions;
using FantasyBaseball.LeagueService.Services;
using FantasyBaseball.LeagueService.Services.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

var BaseballSpecificOrigins = "_BaseballSpecificOrigins";
var SwaggerBasePath = "api";
var SwaggerTitle = "FantasyBaseball.LeagueService";
var SwaggerVersion = "v0";

// Build the App Config
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration["ServiceUrls:PositionEndpoint"] = string.Format(
  builder.Configuration.GetValue<string>("ServiceUrls:PositionEndpoint"),
  builder.Configuration["POSITION_SERVICE_HOST"],
  builder.Configuration["POSITION_SERVICE_VERSION"]);
builder.Configuration["ServiceUrls:PositionHealthEndpoint"] = string.Format(
  builder.Configuration.GetValue<string>("ServiceUrls:PositionHealthEndpoint"),
  builder.Configuration["POSITION_SERVICE_HOST"]);
// Setup Cors
builder.Services.AddCors(options =>
{
  options.AddPolicy(
    name: BaseballSpecificOrigins,
    policy =>
    {
      policy.SetIsOriginAllowed(o =>
      {
        var host = new Uri(o).Host;
        return host == "localhost" || host.Contains("schultz.local");
      });
      policy.AllowAnyHeader();
      policy.AllowAnyMethod();
    });
});
// Setup Database
var connectionString = string.Format(
  builder.Configuration.GetConnectionString("LeagueDatabase"),
  builder.Configuration["LEAGUE_DATABASE_HOST"],
  builder.Configuration["LEAGUE_DATABASE"],
  builder.Configuration["LEAGUE_DATABASE_USER"],
  builder.Configuration["LEAGUE_DATABASE_PASSWORD"]);
builder.Services.AddDbContext<LeagueContext>(options => options.UseNpgsql(connectionString));
// Setup HealthChecks
builder.Services.AddHealthChecks()
  .AddDbContextCheck<LeagueContext>()
  .AddCheck<PositionHealthCheck>("Position");
// Setup Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Setup DI
builder.Services
  // Config
  .AddSingleton(builder.Configuration)
  // Context
  .AddScoped<ILeagueContext>(provider => provider.GetService<LeagueContext>())
  // Cache
  .AddLazyCache()
  // Repos
  .AddScoped<ILeagueRepository, LeagueRepository>()
  // Services
  // .AddScoped<IAddPlayerService, AddPlayerService>()
  // .AddSingleton<ICsvFileReaderService, CsvFileReaderService>()
  // .AddSingleton<ICsvFileWriterService, CsvFileWriterService>()
  // .AddScoped<IDeletePlayerService, DeletePlayerService>()
  // .AddSingleton<IGetPlayerEnumMapService, GetPlayerEnumMapService>()
  // .AddScoped<IGetPlayerService, GetPlayerService>()
  // .AddSingleton<IGetPositionService, GetPositionService>()
  // .AddScoped<IGetTeamsService, GetTeamsService>()
  // .AddScoped<IMergePlayerService, MergePlayerService>()
  // .AddScoped<IMergeStatsService, MergeStatsService>()
  // .AddScoped<IUpdatePlayerService, UpdatePlayerService>();
  ;
// Setup Swagger
builder.Services.AddSwaggerGen(o =>
{
  o.SwaggerDoc(SwaggerVersion, new OpenApiInfo { Title = SwaggerTitle, Version = SwaggerVersion });
  var currentAssembly = Assembly.GetExecutingAssembly();
  currentAssembly.GetReferencedAssemblies()
    .Union([currentAssembly.GetName()])
    .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
    .Where(f => File.Exists(f))
    .ToList()
    .ForEach(f => o.IncludeXmlComments(f));
});
// Setup Controllers
builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

// Build the App
var app = builder.Build();
app.UseCors(BaseballSpecificOrigins);
app.UseHsts();
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/api/health", new HealthCheckOptions { AllowCachingResponses = false });
app.UseSwagger(c => c.RouteTemplate = SwaggerBasePath + "/swagger/{documentName}/swagger.json");
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint($"/{SwaggerBasePath}/swagger/{SwaggerVersion}/swagger.json", $"{SwaggerTitle} - {SwaggerVersion}");
  c.RoutePrefix = $"{SwaggerBasePath}/swagger";
});
// Migrate Database on Startup
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<LeagueContext>().Database.MigrateAsync();
// Start the App
await app.RunAsync();