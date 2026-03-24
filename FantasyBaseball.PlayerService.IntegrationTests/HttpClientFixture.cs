using System;
using System.Net.Http;

namespace FantasyBaseball.PlayerService.IntegrationTests;

public class HttpClientFixture : IDisposable
{
  public HttpClientFixture()
  {
    var host = Environment.GetEnvironmentVariable("PLAYER_SERVICE_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("PLAYER_SERVICE_PORT") ?? "8080";
    Client = new HttpClient { BaseAddress = new Uri($"http://{host}:{port}") };
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
  }
}
