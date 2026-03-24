using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;

namespace FantasyBaseball.LeagueService.Database;

/// <summary>The context object for leagues and their related entities.</summary>
/// <remarks>
///     Initializes a new instance of the Microsoft.EntityFrameworkCore.DbContext class using the specified options. 
///     The Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)
///     method will still be called to allow further configuration of the options.
/// </remarks>
/// <param name="options">The options for this context.</param>
public class LeagueContext(DbContextOptions<LeagueContext> options) : DbContext(options), ILeagueContext
{
  private IDbContextTransaction _transaction;

  /// <summary>A collection of leagues.</summary>
  public DbSet<LeagueEntity> Leagues { get; set; }

  /// <summary>Starts a new database transaction.</summary>
  public async Task BeginTransaction() => _transaction = await Database.BeginTransactionAsync();

  /// <summary>Commits the database transaction.</summary>
  public async Task Commit()
  {
    try { await SaveAndCommit(); }
    finally { await _transaction.DisposeAsync(); }
  }

  /// <summary>Rolls the database transaction back.</summary>
  public async Task Rollback()
  {
    await _transaction.RollbackAsync();
    await _transaction.DisposeAsync();
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    BuildLeagueModel(modelBuilder.Entity<LeagueEntity>());
  }

  private static void BuildLeagueModel(EntityTypeBuilder<LeagueEntity> builder)
  {
    builder.HasKey(l => l.Id).HasName("League_PK");
    builder.Property(l => l.Id).ValueGeneratedOnAdd();
    builder.Property(l => l.Name).HasMaxLength(25);
  }

  private async Task SaveAndCommit()
  {
    await SaveChangesAsync();
    await _transaction.CommitAsync();
  }
}