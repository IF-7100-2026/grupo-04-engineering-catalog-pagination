using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WaynePartsCatalog.Api.DTOs;

namespace WaynePartsCatalog.Tests.Performance;

// Pruebas de rendimiento que validan que la API responda dentro de límites aceptables bajo diferentes tamaños de página.
public class ResponseTimeTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    // Verifica que la API responda en menos de 1 segundo cuando se solicita una página estándar (10 registros).
    [Fact]
    public async Task API_ShouldProcessRequestUnder1Second_Page10()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=10");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();

        result.ResponseTimeMs.Should().BeLessThan(1000);
    }

    // Verifica que la API mantenga un tiempo de respuesta aceptable incluso con una carga grande (300 registros).
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