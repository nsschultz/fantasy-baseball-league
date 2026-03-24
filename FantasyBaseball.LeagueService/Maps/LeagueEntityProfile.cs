using AutoMapper;
using FantasyBaseball.LeagueService.Database.Entities;
using FantasyBaseball.LeagueService.Models;

namespace FantasyBaseball.LeagueService.Maps;

/// <summary>A new profile for the LeagueEntity objects.</summary>
public class LeagueEntityProfile : Profile
{
  /// <summary>Create a new instance of the profile.</summary>
  public LeagueEntityProfile()
  {
    CreateMap<League, LeagueEntity>();
  }
}