using System;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.LeagueService.Database.Entities;
using FantasyBaseball.LeagueService.Database.Repositories;
using FantasyBaseball.LeagueService.Exceptions;
using FantasyBaseball.LeagueService.Models;

namespace FantasyBaseball.LeagueService.Services;

/// <summary>Service for adding a league.</summary>
/// <param name="mapper">Instance of the auto mapper.</param>
/// <param name="leagueRepo">Repo for CRUD functionality regarding to leagues.</param>
public class AddLeagueService(IMapper mapper, ILeagueRepository leagueRepo) : IAddLeagueService
{
  /// <summary>Adds the given league.</summary>
  /// <param name="league">The league to add.</param>
  /// <returns>The id of the newly created object.</returns>
  public async Task<Guid> AddLeague(League league)
  {
    var existingLeague = await leagueRepo.GetLeagueById(league.Id);
    if (existingLeague != null) throw new BadRequestException("This league already exists");
    var entity = mapper.Map<LeagueEntity>(league);
    await leagueRepo.AddLeague(entity);
    return entity.Id;
  }
}