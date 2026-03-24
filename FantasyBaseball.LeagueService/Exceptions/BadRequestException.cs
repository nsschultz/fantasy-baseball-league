using System;

namespace FantasyBaseball.LeagueService.Exceptions;

/// <summary>Exception that is thrown when there is problem with the request.</summary>
public class BadRequestException : Exception
{
  /// <summary>Initializes a new instance of the exception class with the default error message.</summary>
  public BadRequestException() : base("The given request was invalid.") { }

  /// <summary>Initializes a new instance of the exception class with a specified error message.</summary>
  /// <param name="message">The message that describes the error.</param>
  public BadRequestException(string message) : base(message) { }

  /// <summary>Initializes a new instance of the exception class with a specified error message.</summary>
  /// <param name="message">The message that describes the error.</param>
  /// <param name="innerException">
  ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) 
  ///     if no inner exception is specified.
  /// </param>
  public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}