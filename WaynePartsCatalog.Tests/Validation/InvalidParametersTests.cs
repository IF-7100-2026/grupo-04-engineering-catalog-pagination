using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace WaynePartsCatalog.Tests.Validation;

// Pruebas de validación que verifican el comportamiento del API ante parámetros inválidos o casos borde.
public class InvalidParametersTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    // Verifica que un número de página negativo retorne BadRequest.
    [Fact]
    public async Task Invalid_Page_ShouldFail()
    {
        var response = await Client.GetAsync("/api/parts?page=-1&size=10");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Verifica que un tamaño de página igual a cero sea rechazado.
    [Fact]
    public async Task Invalid_SizeZero_ShouldFail()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=0");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Verifica que tamaños de página excesivamente grandes sean rechazados.
    [Fact]
    public async Task Invalid_SizeTooLarge_ShouldFail()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=9999");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Verifica que un material vacío o con espacios no rompa la API.
    [Fact]
    public async Task Invalid_Material_EmptyString_ShouldStillWorkOrBeIgnored()
    {
        var response = await Client.GetAsync("/api/parts?material=   ");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();
    }

    // Verifica que una descripción vacía no genere errores.
    [Fact]
    public async Task Invalid_Description_Empty_ShouldNotCrash()
    {
        var response = await Client.GetAsync("/api/parts?descriptionContains=");

        response.EnsureSuccessStatusCode();
    }

    // Verifica que un rango de fechas invertido no rompa la API.
    [Fact]
    public async Task Invalid_DateRange_Inverted_ShouldStillReturnOk()
    {
        var response = await Client.GetAsync(
            "/api/parts?manufactureDateFrom=2030-01-01&manufactureDateTo=2020-01-01");

        response.EnsureSuccessStatusCode();
    }

    // Verifica que combinaciones de parámetros inválidos no provoquen fallos.
    [Fact]
    public async Task Invalid_CombinedParameters_ShouldNotCrash()
    {
        var response = await Client.GetAsync(
            "/api/parts?page=0&size=10&weightFrom=-100&sizeTo=-50&material=");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();
    }

    // Verifica que cuando no se envían parámetros, la API use valores por defecto.
    [Fact]
    public async Task MissingParameters_ShouldReturnDefaultBehavior()
    {
        var response = await Client.GetAsync("/api/parts");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Page.Should().Be(0);
        result.Size.Should().Be(10);
    }
}