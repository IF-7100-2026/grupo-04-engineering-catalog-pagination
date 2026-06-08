using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WaynePartsCatalog.Tests.Integration;

// Pruebas de integración que validan combinaciones de filtros simultáneos en el endpoint del catálogo de partes.
public class CombinationFilterTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    // Verifica que el filtro combinado de material + rango de peso funcione correctamente y solo devuelva resultados válidos.
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

    // Verifica que los filtros de fecha de fabricación y timestamp de registro funcionen correctamente en conjunto.
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

    // Verifica que múltiples filtros combinados (material, peso, tamaño y descripción) se apliquen correctamente sin afectar la consistencia del resultado.
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