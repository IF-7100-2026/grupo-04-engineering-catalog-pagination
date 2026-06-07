using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WaynePartsCatalog.Api.DTOs;

namespace WaynePartsCatalog.Tests.Performance;

public class ResponseTimeTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    [Fact]
    public async Task API_ShouldProcessRequestUnder1Second_Page10()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=10");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();

        result.ResponseTimeMs.Should().BeLessThan(1000);
    }

    [Fact]
    public async Task API_ShouldProcessRequestUnder1Second_Page300()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=300");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();

        result.ResponseTimeMs.Should().BeLessThan(3000);
    }
}