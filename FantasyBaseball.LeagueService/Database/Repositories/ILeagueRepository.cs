using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Database.Entities;

namespace FantasyBaseball.LeagueService.Database.Repositories;

/// <summary>Repo for CRUD functionality regarding to leagues.</summary>
public interface ILeagueRepository
{
  /// <summary>Adds the given league to the database.</summary>
  /// <param name="league">The league data.</param>
  Task AddLeague(LeagueEntity league);

  /// <summary>Deletes the given league from the database.</summary>
  /// <param name="league">The league data.</param>
  Task DeleteLeague(LeagueEntity league);

  /// <summary>Finds a league matching the given id or null if there is no match.</summary>
  /// <param name="id">The guid of the league to find.</param>
  /// <returns>The league matching the given id.</returns>
  Task<LeagueEntity> GetLeagueById(Guid id);

  /// <summary>Gets a list of league entities from the database.</summary>
  /// <returns>A list of the leagues.</returns>
  Task<List<LeagueEntity>> GetLeagues();

  /// <summary>Updates the given league.</summary>
  /// <param name="league">The league data.</param>
  Task UpdateLeague(LeagueEntity league);
}