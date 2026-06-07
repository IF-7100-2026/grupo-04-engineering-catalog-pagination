using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WaynePartsCatalog.Api.Data;
using WaynePartsCatalog.Api.DTOs;

namespace WaynePartsCatalog.Api.Services;

public class PartService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 300;

    private readonly AppDbContext _context;

    public PartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponseDto<PartResponseDto>> GetPartsAsync(int page, int size)
    {
        ValidatePaginationParameters(page, size);

        // Se mide el tiempo que tarda el backend en procesar la consulta.
        var stopwatch = Stopwatch.StartNew();

        // Consulta base del catalogo.
        // BORRAR: aqui se pueden agregar los filtros antes del conteo y la paginacion.
        var query = _context.EngineeringParts
            .AsNoTracking()
            .OrderBy(part => part.PartId);

        var totalElements = await query.LongCountAsync();

        // Se aplica la paginacion en la base de datos.
        var parts = await query
            .Skip(page * size)
            .Take(size)
            .Select(part => new PartResponseDto
            {
                PartId = part.PartId,
                ManufactureDate = part.ManufactureDate,
                RegistrationTimestamp = part.RegistrationTimestamp,
                WeightKg = part.WeightKg,
                SizeMeters = part.SizeMeters,
                PartType = part.PartType,
                Material = part.Material,
                LongDescription = part.LongDescription
            })
            .ToListAsync();

        stopwatch.Stop();

        var totalPages = CalculateTotalPages(totalElements, size);

        return new PaginatedResponseDto<PartResponseDto>
        {
            Content = parts,
            Page = page,
            Size = size,
            TotalElements = totalElements,
            TotalPages = totalPages,
            HasNext = page + 1 < totalPages,
            HasPrevious = page > 0,
            ResponseTimeMs = stopwatch.ElapsedMilliseconds
        };
    }

    private static void ValidatePaginationParameters(int page, int size)
    {
        if (page < 0)
        {
            throw new ArgumentException("Page number must be greater than or equal to 0.");
        }

        if (size < 1)
        {
            throw new ArgumentException("Page size must be greater than or equal to 1.");
        }

        if (size > MaxPageSize)
        {
            throw new ArgumentException($"Page size cannot be greater than {MaxPageSize}.");
        }
    }

    private static int CalculateTotalPages(long totalElements, int size)
    {
        if (totalElements == 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(totalElements / (double)size);
    }
}