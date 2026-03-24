using System;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Models;

namespace FantasyBaseball.LeagueService.Services;

/// <summary>Service for adding a league.</summary>
public interface IAddLeagueService
{
  /// <summary>Adds the given league.</summary>
  /// <param name="league">The league to add.</param>
  /// <returns>The id of the newly created object.</returns>
  public Task<Guid> AddLeague(League league);
}