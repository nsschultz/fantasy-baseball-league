using System;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.LeagueService.Database.Entities;
using FantasyBaseball.LeagueService.Database.Repositories;
using FantasyBaseball.LeagueService.Exceptions;
using FantasyBaseball.LeagueService.Maps;
using FantasyBaseball.LeagueService.Models;
using FantasyBaseball.LeagueService.Services;
using Moq;
using Xunit;

namespace FantasyBaseball.LeagueService.UnitTests.Services;

public class AddLeagueServiceTest
{
  [Fact]
  public async Task AddLeagueTestExistingIdException()
  {
    var id = Guid.NewGuid();
    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new LeagueEntityProfile())).CreateMapper();
    var league = new League { Id = id, Name = "Test League" };
    var leagueRepo = new Mock<ILeagueRepository>();
    leagueRepo.Setup(o => o.GetLeagueById(It.Is<Guid>(g => g == id))).ReturnsAsync(new LeagueEntity { });
    await Assert.ThrowsAsync<BadRequestException>(async () => await new AddLeagueService(mapper, leagueRepo.Object).AddLeague(league));
  }

  [Fact]
  public async Task AddLeagueTestValid()
  {
    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new LeagueEntityProfile())).CreateMapper();
    var id = Guid.NewGuid();
    var league = new League { Id = id, Name = "Test League" };
    var leagueRepo = new Mock<ILeagueRepository>();
    leagueRepo.Setup(o => o.GetLeagueById(It.Is<Guid>(g => g == id))).ReturnsAsync((LeagueEntity)null);
    leagueRepo.Setup(o => o.AddLeague(It.Is<LeagueEntity>(p => p.Id == id))).Returns(Task.FromResult(0));
    var newId = await new AddLeagueService(mapper, leagueRepo.Object).AddLeague(league);
    Assert.Equal(id, newId);
  }
}