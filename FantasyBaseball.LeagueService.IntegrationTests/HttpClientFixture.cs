using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FantasyBaseball.LeagueService.IntegrationTests;

public class HttpClientFixture : IDisposable
{
  private readonly WebApplicationFactory<Program> _application;

  public HttpClientFixture()
  {
    _application = new WebApplicationFactory<Program>();
    Client = _application.CreateClient();
  }

  public HttpClient Client { get; private set; }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing) return;
    Client.Dispose();
    _application.Dispose();
  }
}
