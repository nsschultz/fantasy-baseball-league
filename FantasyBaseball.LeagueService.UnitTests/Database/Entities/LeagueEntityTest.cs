using System;
using FantasyBaseball.LeagueService.Database.Entities;
using Xunit;

namespace FantasyBaseball.LeagueService.UnitTests.Database.Entities;

public class LeagueEntityTest
{
  [Fact]
  public void DefaultsSetTest()
  {
    var obj = new LeagueEntity();
    Assert.Equal(Guid.Empty, obj.Id);
    Assert.Null(obj.Name);
  }
}