using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyBaseball.LeagueService.Models;
using Xunit;

namespace FantasyBaseball.LeagueService.IntegrationTests;

public class LeagueIntegrationTests(HttpClientFixture fixture) : IClassFixture<HttpClientFixture>
{
  private readonly HttpClientFixture _fixture = fixture;
  private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

  [Theory]
  [InlineData("/api/health")]
  [InlineData("/api/swagger/index.html")]
  public async Task GetSimpleTests(string url)
  {
    var httpResponse = await _fixture.Client.GetAsync(url);
    Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
  }
}
