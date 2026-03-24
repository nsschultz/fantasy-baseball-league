using System;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Controllers.V0;
using FantasyBaseball.LeagueService.Exceptions;
using FantasyBaseball.LeagueService.Models;
using FantasyBaseball.LeagueService.Services;
using Moq;
using Xunit;

namespace FantasyBaseball.LeagueService.UnitTests.Controllers.V0;

public class LeagueAddControllerTest
{
  [Fact]
  public async Task AddLeagueTest()
  {
    var id = Guid.NewGuid();
    var addService = new Mock<IAddLeagueService>();
    addService.Setup(o => o.AddLeague(It.IsAny<League>())).ReturnsAsync(id);
    var newId = await new LeagueAddController(addService.Object).AddLeague(new League { });
    Assert.Equal(id, newId);
  }

  [Fact]
  public async Task AddLeagueTestExistingLeagueId() =>
    await Assert.ThrowsAsync<BadRequestException>(() => new LeagueAddController(null).AddLeague(new League { Id = Guid.NewGuid() }));

  [Fact]
  public async Task AddLeagueTestNullLeague() => await Assert.ThrowsAsync<BadRequestException>(() => new LeagueAddController(null).AddLeague(null));
}