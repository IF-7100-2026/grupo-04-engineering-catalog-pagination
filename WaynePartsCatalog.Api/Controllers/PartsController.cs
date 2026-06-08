using Microsoft.AspNetCore.Mvc;
using WaynePartsCatalog.Api.DTOs;
using WaynePartsCatalog.Api.Services;

namespace WaynePartsCatalog.Api.Controllers;

[ApiController]
[Route("api/parts")]
public class PartsController(PartService partService) : ControllerBase
{
    private readonly PartService _partService = partService;

    //Obtiene una lista paginada de partes del catálogo aplicando filtros opcionales.
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<PartResponseDto>>> GetParts(
        [FromQuery] int page = 0,
        [FromQuery] int size = 10,

        [FromQuery] DateOnly? manufactureDateFrom = null,
        [FromQuery] DateOnly? manufactureDateTo = null,

        [FromQuery] DateTime? registrationFrom = null,
        [FromQuery] DateTime? registrationTo = null,

        [FromQuery] int? weightFrom = null,
        [FromQuery] int? weightTo = null,

        [FromQuery] decimal? sizeFrom = null,
        [FromQuery] decimal? sizeTo = null,

        [FromQuery] string? material = null,
        [FromQuery] string? partType = null,

        [FromQuery] string? descriptionContains = null)
    {
        try
        {
            // Se construye el DTO de filtros con los parámetros recibidos por query string
            var filters = new PartFilterDto
            {
                ManufactureDateFrom = manufactureDateFrom,
                ManufactureDateTo = manufactureDateTo,

                RegistrationFrom = registrationFrom,
                RegistrationTo = registrationTo,

                WeightFrom = weightFrom,
                WeightTo = weightTo,

                SizeFrom = sizeFrom,
                SizeTo = sizeTo,

                Material = material,
                PartType = partType,

                DescriptionContains = descriptionContains
            };

            // Se delega la lógica de negocio al servicio
            var response = await _partService.GetPartsAsync(
                page,
                size,
                filters);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            // Si los parámetros de paginación son inválidos, se responde 400
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }
}