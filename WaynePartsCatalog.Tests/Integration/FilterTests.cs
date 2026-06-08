using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.ComponentModel.DataAnnotations;

namespace WaynePartsCatalog.Tests.Integration;

// Pruebas de integración para validar el comportamiento individual de cada filtro del catálogo de partes.
public class FilterTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    // Valida el filtro por rango de fecha de fabricación.
    [Fact]
    public async Task Filter_ByManufactureDate_ShouldWork()
    {
        var response = await Client.GetAsync(
            "/api/parts?manufactureDateFrom=2024-01-01&manufactureDateTo=2025-01-01");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.ManufactureDate >= new DateOnly(2024, 1, 1) &&
            p.ManufactureDate <= new DateOnly(2025, 1, 1));
    }

    // Valida el filtro por rango de timestamp de registro.
    [Fact]
    public async Task Filter_ByTimestamp_ShouldWork()
    {
        var response = await Client.GetAsync(
            "/api/parts?registrationFrom=2024-01-01T00:00:00Z&registrationTo=2025-01-01T00:00:00Z");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.RegistrationTimestamp >= new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) &&
            p.RegistrationTimestamp <= new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }

    // Valida el filtro por rango de peso.
    [Fact]
    public async Task Filter_ByWeight_ShouldWork()
    {
        var response = await Client.GetAsync("/api/parts?weightFrom=1000&weightTo=2000");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.WeightKg >= 1000 && p.WeightKg <= 2000);
    }

    // Valida el filtro por rango de tamaño.
    [Fact]
    public async Task Filter_BySize_ShouldWork()
    {
        var response = await Client.GetAsync("/api/parts?sizeFrom=10&sizeTo=50");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.SizeMeters >= 10 && p.SizeMeters <= 50);
    }

    // Valida el filtro exacto por material.
    [Fact]
    public async Task Filter_ByMaterial_ShouldWork()
    {
        var response = await Client.GetAsync("/api/parts?material=Steel");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.Material == "Steel");
    }

    // Valida el filtro de búsqueda parcial en la descripción.
    [Fact]
    public async Task Filter_ByDescription_ShouldWork()
    {
        var response = await Client.GetAsync("/api/parts?descriptionContains=engine");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Content.Should().OnlyContain(p =>
            p.LongDescription.Contains("engine", StringComparison.OrdinalIgnoreCase));
    }
}