using System;
using System.Linq;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Database;
using FantasyBaseball.LeagueService.Database.Entities;
using FantasyBaseball.LeagueService.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace FantasyBaseball.LeagueService.UnitTests.Database.Repositories;

public class LeagueRepositoryTest : IDisposable
{
  private static readonly Guid LeagueMatchingId = Guid.NewGuid();
  private static readonly Guid LeagueMissingId = Guid.NewGuid();
  private readonly LeagueContext _context;

  public LeagueRepositoryTest() => _context = CreateContext().Result;

  [Fact]
  public async Task AddLeagueTestExistingIdException()
  {
    var league = await _context.Leagues.FindAsync(LeagueMatchingId);
    await Assert.ThrowsAsync<ArgumentException>(async () => await new LeagueRepository(_context).AddLeague(league));
    Assert.Equal(3, await _context.Leagues.CountAsync());
  }

  [Fact]
  public async Task AddLeagueTestValid()
  {
    var league = new LeagueEntity { Name = "New League" };
    await new LeagueRepository(_context).AddLeague(league);
    Assert.Equal(4, await _context.Leagues.CountAsync());
  }

  [Fact]
  public async Task DeleteLeagueTestMissingIdException()
  {
    var league = new LeagueEntity { Id = LeagueMissingId, Name = "Missing League" };
    await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await new LeagueRepository(_context).DeleteLeague(league));
    Assert.Equal(3, await _context.Leagues.CountAsync());
  }

  [Fact]
  public async Task DeleteLeagueTestValid()
  {
    var league = await _context.Leagues.FindAsync(LeagueMatchingId);
    await new LeagueRepository(_context).DeleteLeague(league);
    Assert.Equal(2, await _context.Leagues.CountAsync());
  }

  [Fact] public async Task GetLeagueByIdNull() => Assert.Null(await new LeagueRepository(_context).GetLeagueById(LeagueMissingId));

  [Fact] public async Task GetLeagueByIdValid() => Assert.Equal(LeagueMatchingId, (await new LeagueRepository(_context).GetLeagueById(LeagueMatchingId)).Id);

  [Fact]
  public async Task GetLeagueEntitiesTest()
  {
    var leagues = await new LeagueRepository(_context).GetLeagues();
    Assert.Equal(3, leagues.Count);
  }

  [Fact]
  public async Task UpdatePlayerTestMissingIdException()
  {
    Assert.Equal("League 2", (await _context.Leagues.FindAsync(LeagueMatchingId)).Name);
    var league = new LeagueEntity { Id = LeagueMissingId, Name = "Missing League" };
    await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await new LeagueRepository(_context).UpdateLeague(league));
    Assert.Equal("League 2", (await _context.Leagues.FindAsync(LeagueMatchingId)).Name);
  }

  [Fact]
  public async Task UpdatePlayerTestValid()
  {
    Assert.Equal("League 2", (await _context.Leagues.FindAsync(LeagueMatchingId)).Name);
    var league = await _context.Leagues.FindAsync(LeagueMatchingId);
    league.Name = "Updated League";
    await new LeagueRepository(_context).UpdateLeague(league);
    Assert.Equal("Updated League", (await _context.Leagues.FindAsync(LeagueMatchingId)).Name);
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing) return;
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

  private static async Task<LeagueContext> CreateContext()
  {
    var options = new DbContextOptionsBuilder<LeagueContext>()
      .UseInMemoryDatabase(databaseName: "GetLeaguesTest")
      .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
      .Options;
    var context = new LeagueContext(options);
    await context.Database.EnsureCreatedAsync();
    await context.AddRangeAsync(
      new LeagueEntity { Name = "League 1" },
      new LeagueEntity { Id = LeagueMatchingId, Name = "League 2" },
      new LeagueEntity { Name = "League 3" }
    );
    await context.SaveChangesAsync();
    Assert.Equal(3, await context.Leagues.CountAsync());
    return context;
  }
}