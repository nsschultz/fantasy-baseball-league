using System;
using System.Text.Json.Serialization;

namespace FantasyBaseball.LeagueService.Models;

/// <summary>All of the information that makes up a baseball player.</summary>
public class League
{
  /// <summary>The unique id of the league.</summary>
  public Guid Id { get; set; }
  /// <summary>The name of the league.</summary>
  [JsonRequired] public string Name { get; set; }
}