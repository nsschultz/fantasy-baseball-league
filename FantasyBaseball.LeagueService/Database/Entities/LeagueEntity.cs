using System;

namespace FantasyBaseball.LeagueService.Database.Entities;

/// <summary>Entity for saving/retrieving a League.</summary>
public class LeagueEntity
{
  /// <summary>The unique identifier for the league.</summary>
  public Guid Id { get; set; }

  /// <summary>The name of the league.</summary>
  public string Name { get; set; }
}