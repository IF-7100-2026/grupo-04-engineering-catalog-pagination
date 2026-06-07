using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.Json;

using WaynePartsCatalog.Api.DTOs;

namespace WaynePartsCatalog.Tests;

public abstract class TestBase(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client = factory.CreateClient();

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected static async Task<PaginatedResponseDto<PartResponseDto>> ReadResponse(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<PaginatedResponseDto<PartResponseDto>>(
            json,
            JsonOptions);

        result.Should().NotBeNull();

        return result!;
    }

    protected async Task<PaginatedResponseDto<PartResponseDto>> GetAsync(string url)
    {
        var response = await Client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<PaginatedResponseDto<PartResponseDto>>(
            json,
            JsonOptions);

        result.Should().NotBeNull();

        return result!;
    }
}