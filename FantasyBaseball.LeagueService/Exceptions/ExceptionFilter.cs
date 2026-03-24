using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FantasyBaseball.LeagueService.Exceptions;

/// <summary>A filter for returning back HTTP errors.</summary>
public class ExceptionFilter : IActionFilter
{
  /// <summary>Called after the action executes, before the action result.</summary>
  /// <param name="context">The ActionExecutedContext.</param>
  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Exception is BadRequestException bre)
    {
      context.Result = new ObjectResult(bre.Message) { StatusCode = 400 };
      context.ExceptionHandled = true;
    }
    if (context.Exception is ServiceException se)
    {
      context.Result = new ObjectResult(se.Message) { StatusCode = 500 };
      context.ExceptionHandled = true;
    }
  }

  /// <summary>Called before the action executes, after model binding is complete.</summary>
  /// <param name="context">The ActionExecutingContext.</param>
  public void OnActionExecuting(ActionExecutingContext context)
  {
    //Exception filtering isn't applied on executing actions
  }
}