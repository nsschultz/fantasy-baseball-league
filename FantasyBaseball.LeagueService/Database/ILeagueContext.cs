using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.LeagueService.Database;

/// <summary>The context object for leagues and their related entities.</summary>
public interface ILeagueContext
{
  /// <summary>A collection of leagues.</summary>
  DbSet<LeagueEntity> Leagues { get; set; }

  /// <summary>Starts a new database transaction.</summary>
  Task BeginTransaction();

  /// <summary>Commits the database transaction.</summary>
  Task Commit();

  /// <summary>Rolls the database transaction back.</summary>
  Task Rollback();
}