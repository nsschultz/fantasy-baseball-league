using System;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Exceptions;
using FantasyBaseball.LeagueService.Models;
using FantasyBaseball.LeagueService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyBaseball.LeagueService.Controllers.V0;

/// <summary>Endpoint for adding a league's data.</summary>
/// <param name="addService">Service for adding a league.</param>
[Route("api/v0/league")]
[ApiController]
public class LeagueAddController(IAddLeagueService addService) : ControllerBase
{
  /// <summary>Adds the given league.</summary>
  /// <param name="league">The object containing all of the league's data (non-changed data must be included as well).</param>
  /// <returns>The id of the newly created object.</returns>
  [HttpPost]
  public async Task<Guid> AddLeague([FromBody] League league)
  {
    if (league == null) throw new BadRequestException("League not set");
    if (league.Id != Guid.Empty) throw new BadRequestException("The id should not be set");
    return await addService.AddLeague(league);
  }
}