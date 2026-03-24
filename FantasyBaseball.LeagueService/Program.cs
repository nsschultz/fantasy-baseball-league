using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

var BaseballSpecificOrigins = "_BaseballSpecificOrigins";
var SwaggerBasePath = "api";
var SwaggerTitle = "FantasyBaseball.LeagueService";
var SwaggerVersion = "v1";

// Build the App Config
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
// Setup Cors
builder.Services.AddCors(options =>
{
  options.AddPolicy(
    name: BaseballSpecificOrigins,
    policy =>
    {
      policy.SetIsOriginAllowed(o =>
      {
        var host = new System.Uri(o).Host;
        return host == "localhost" || host.Contains("schultz.local");
      });
      policy.AllowAnyHeader();
      policy.AllowAnyMethod();
    });
});
// Setup HealthChecks
builder.Services.AddHealthChecks();
// Setup Automapper
builder.Services.AddAutoMapper(System.AppDomain.CurrentDomain.GetAssemblies());
// Setup DI
builder.Services
  // Config
  .AddSingleton(builder.Configuration)
  // Cache
  .AddLazyCache();
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
builder.Services.AddControllers();

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
// Start the App
await app.RunAsync();
