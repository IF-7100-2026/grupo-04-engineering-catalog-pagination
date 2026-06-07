using Microsoft.AspNetCore.Mvc;
using WaynePartsCatalog.Api.DTOs;
using WaynePartsCatalog.Api.Services;

namespace WaynePartsCatalog.Api.Controllers;

[ApiController]
[Route("api/parts")]
public class PartsController : ControllerBase
{
    private readonly PartService _partService;

    public PartsController(PartService partService)
    {
        _partService = partService;
    }

    // Endpoint principal para consultar partes del catalogo con paginacion.
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<PartResponseDto>>> GetParts(
        [FromQuery] int page = 0,
        [FromQuery] int size = 10)
    {
        try
        {
            // BORRAR: pendiente agregar filtros de busqueda.
            var response = await _partService.GetPartsAsync(page, size);
            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            // Respuesta controlada para parametros invalidos.
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }
}