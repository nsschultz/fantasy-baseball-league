using System;
using FantasyBaseball.LeagueService.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace FantasyBaseball.LeagueService.UnitTests.Exceptions;

public class ExceptionFilterTest
{
  [Fact] public void BadRequestExceptionTest() => ValidateExceptionHandling(new BadRequestException("Someone did something stupid"), 400);

  [Fact] public void ServiceExceptionTest() => ValidateExceptionHandling(new ServiceException("Something horrible happened"), 500);

  private static void ValidateExceptionHandling(Exception e, int statusCode)
  {
    var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
    var context = new ActionExecutedContext(actionContext, [], controller: null) { Exception = e };
    new ExceptionFilter().OnActionExecuted(context);
    var result = context.Result as ObjectResult;
    Assert.Equal(statusCode, result.StatusCode);
    Assert.Equal(e.Message, result.Value);
  }
}