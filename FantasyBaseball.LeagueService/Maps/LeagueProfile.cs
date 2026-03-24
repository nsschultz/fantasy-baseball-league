using AutoMapper;
using FantasyBaseball.LeagueService.Database.Entities;
using FantasyBaseball.LeagueService.Models;

namespace FantasyBaseball.LeagueService.Maps;

/// <summary>A new profile for the League objects.</summary>
public class LeagueProfile : Profile
{
  /// <summary>Create a new instance of the profile.</summary>
  public LeagueProfile()
  {
    CreateMap<LeagueEntity, League>();
  }
}