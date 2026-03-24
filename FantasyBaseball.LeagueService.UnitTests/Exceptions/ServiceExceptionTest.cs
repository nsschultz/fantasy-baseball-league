using System;
using FantasyBaseball.LeagueService.Exceptions;
using Xunit;

namespace FantasyBaseball.LeagueService.UnitTests.Exceptions;

public class ServiceExceptionTest
{
  [Fact] public void EqualsDefault() => Assert.Equal("There was an unknown exception with the service.", new ServiceException().Message);

  [Fact]
  public void EqualsInnerException()
  {
    var exception = new ServiceException("message", new Exception("inner message"));
    Assert.Equal("message", exception.Message);
    Assert.Equal("inner message", exception.InnerException.Message);
  }

  [Fact] public void EqualsMessage() => Assert.Equal("message", new ServiceException("message").Message);
}