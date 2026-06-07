using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WaynePartsCatalog.Tests.Integration;

public class CombinationFilterTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    [Fact]
    public async Task Filter_MaterialAndWeight_ShouldWork()
    {
        var response = await Client.GetAsync(
            "/api/parts?material=Steel&weightFrom=1000&weightTo=2000");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().NotBeEmpty();

        result.Content.Should().OnlyContain(p =>
            p.Material == "Steel" &&
            p.WeightKg >= 1000 &&
            p.WeightKg <= 2000);
    }

    [Fact]
    public async Task Filter_DateAndTimestamp_ShouldWork()
    {
        var response = await Client.GetAsync(
            "/api/parts?manufactureDateFrom=2024-01-01&registrationFrom=2024-01-01T00:00:00Z");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.ManufactureDate >= new DateOnly(2024, 1, 1) &&
            p.RegistrationTimestamp >= new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public async Task Filter_AllCombined_ShouldWork()
    {
        var response = await Client.GetAsync(
            "/api/parts?material=Steel&weightFrom=1000&weightTo=2000&sizeFrom=10&sizeTo=50&descriptionContains=engine");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.Material == "Steel" &&
            p.WeightKg >= 1000 &&
            p.WeightKg <= 2000 &&
            p.SizeMeters >= 10 &&
            p.SizeMeters <= 50 &&
            p.LongDescription.Contains("engine", StringComparison.OrdinalIgnoreCase));
    }
}