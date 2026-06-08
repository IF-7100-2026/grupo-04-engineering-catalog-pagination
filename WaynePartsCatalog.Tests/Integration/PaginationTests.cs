using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WaynePartsCatalog.Tests.Integration;

// Pruebas de integración para validar el comportamiento de la paginación en el endpoint del catálogo de partes.
public class PaginationTests(WebApplicationFactory<Program> factory)
    : TestBase(factory)
{
    // Valida la paginación por defecto (page=0, size=10) y la consistencia de los metadatos devueltos por la API.
    [Fact]
    public async Task DefaultPagination_ShouldReturn10Records_AndValidMetadata()
    {
        var result = await ReadResponse(
            await Client.GetAsync("/api/parts?page=0&size=10"));

        result.Content.Should().NotBeNull();

        //Tamaño de página 
        result.Size.Should().Be(10);
        result.Page.Should().Be(0);

        //Metadatos de paginación 
        result.TotalElements.Should().BeGreaterThanOrEqualTo(0);

        var expectedTotalPages = result.TotalElements == 0
            ? 0
            : (int)Math.Ceiling(result.TotalElements / (double)result.Size);

        result.TotalPages.Should().Be(expectedTotalPages);

        result.HasPrevious.Should().BeFalse();

        result.HasNext.Should().Be(
            result.TotalPages > 1 &&
            result.Page < result.TotalPages - 1);
    }

    // Valida que el tamaño máximo de página permitido (300) sea respetado y que los metadatos sean consistentes.
    [Fact]
    public async Task Pagination_300_ShouldRespectMaxPageSize_AndReturnValidMetadata()
    {
        var result = await ReadResponse(
            await Client.GetAsync("/api/parts?page=0&size=300"));

        result.Content.Should().NotBeNull();

        //Tamaño solicitado
        result.Size.Should().Be(300);
        result.Page.Should().Be(0);

        //Consistencia de metadata
        result.TotalElements.Should().BeGreaterThanOrEqualTo(0);

        var expectedTotalPages = result.TotalElements == 0
            ? 0
            : (int)Math.Ceiling(result.TotalElements / (double)result.Size);

        result.TotalPages.Should().Be(expectedTotalPages);

        result.HasPrevious.Should().BeFalse();

        result.HasNext.Should().Be(
            result.TotalPages > 1 &&
            result.Page < result.TotalPages - 1);
    }
}