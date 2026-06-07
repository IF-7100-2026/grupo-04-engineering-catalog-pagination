using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net;

namespace WaynePartsCatalog.Tests.Validation;

public class InvalidParametersTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    [Fact]
    public async Task Invalid_Page_ShouldFail()
    {
        var response = await Client.GetAsync("/api/parts?page=-1&size=10");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Invalid_SizeZero_ShouldFail()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=0");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Invalid_SizeTooLarge_ShouldFail()
    {
        var response = await Client.GetAsync("/api/parts?page=0&size=9999");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    //material inválido (vacío o solo espacios)
    [Fact]
    public async Task Invalid_Material_EmptyString_ShouldStillWorkOrBeIgnored()
    {
        var response = await Client.GetAsync("/api/parts?material=   ");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();
    }

    //String vacío en descripción
    [Fact]
    public async Task Invalid_Description_Empty_ShouldNotCrash()
    {
        var response = await Client.GetAsync("/api/parts?descriptionContains=");

        response.EnsureSuccessStatusCode();
    }

    // Fechas invertidas
    [Fact]
    public async Task Invalid_DateRange_Inverted_ShouldStillReturnOk()
    {
        var response = await Client.GetAsync(
            "/api/parts?manufactureDateFrom=2030-01-01&manufactureDateTo=2020-01-01");

        response.EnsureSuccessStatusCode();
    }

    //Combinación inválida múltiple
    [Fact]
    public async Task Invalid_CombinedParameters_ShouldNotCrash()
    {
        var response = await Client.GetAsync(
            "/api/parts?page=0&size=10&weightFrom=-100&sizeTo=-50&material=");

        response.EnsureSuccessStatusCode();

        var result = await ReadResponse(response);

        result.Should().NotBeNull();
    }

    //Null-edge simulado (parámetros omitidos)
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