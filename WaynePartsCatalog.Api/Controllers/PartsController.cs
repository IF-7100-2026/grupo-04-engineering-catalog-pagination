using Microsoft.AspNetCore.Mvc;
using WaynePartsCatalog.Api.DTOs;
using WaynePartsCatalog.Api.Services;

namespace WaynePartsCatalog.Api.Controllers;

[ApiController]
[Route("api/parts")]
public class PartsController(PartService partService) : ControllerBase
{
    private readonly PartService _partService = partService;

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

            var response = await _partService.GetPartsAsync(
                page,
                size,
                filters);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }
}