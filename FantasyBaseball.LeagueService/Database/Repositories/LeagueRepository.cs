using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.LeagueService.Database.Repositories;

/// <summary>Repo for CRUD functionality regarding to leagues.</summary>
/// <param name="context">The league context.</param>
public class LeagueRepository(ILeagueContext context) : ILeagueRepository
{
  /// <summary>Adds the given league to the database.</summary>
  /// <param name="league">The league data.</param>
  public async Task AddLeague(LeagueEntity league)
  {
    try
    {
      await context.BeginTransaction();
      await context.Leagues.AddAsync(league);
      await context.Commit();
    }
    catch (Exception)
    {
      await context.Rollback();
      throw;
    }
  }

  /// <summary>Deletes the given league from the database.</summary>
  /// <param name="league">The league data.</param>
  public async Task DeleteLeague(LeagueEntity league)
  {
    try
    {
      await context.BeginTransaction();
      context.Leagues.Remove(league);
      await context.Commit();
    }
    catch (Exception)
    {
      await context.Rollback();
      throw;
    }
  }

  /// <summary>Finds a league matching the given id or null if there is no match.</summary>
  /// <param name="id">The guid of the league to find.</param>
  /// <returns>The league matching the given id.</returns>
  public async Task<LeagueEntity> GetLeagueById(Guid id) => await GetQueryable().FirstOrDefaultAsync(l => l.Id == id);

  /// <summary>Gets a list of league entities from the database.</summary>
  /// <returns>A list of the leagues.</returns>
  public async Task<List<LeagueEntity>> GetLeagues() =>
    await GetQueryable().ToListAsync();

  /// <summary>Updates the given league.</summary>
  /// <param name="league">The league data.</param>
  public async Task UpdateLeague(LeagueEntity league)
  {
    try
    {
      await context.BeginTransaction();
      context.Leagues.Update(league);
      await context.Commit();
    }
    catch (Exception)
    {
      await context.Rollback();
      throw;
    }
  }

  private IQueryable<LeagueEntity> GetQueryable() => context.Leagues;
}