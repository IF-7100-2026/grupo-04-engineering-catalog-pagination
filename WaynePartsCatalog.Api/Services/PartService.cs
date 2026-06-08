using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WaynePartsCatalog.Api.Data;
using WaynePartsCatalog.Api.DTOs;
using WaynePartsCatalog.Api.Specifications;

namespace WaynePartsCatalog.Api.Services;

public class PartService(AppDbContext context)
{
    private readonly AppDbContext _context = context;


    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 300;

    public async Task<PaginatedResponseDto<PartResponseDto>> GetPartsAsync(int page, int size, PartFilterDto filters)
    {
        ValidatePaginationParameters(page, size);

        // Se mide el tiempo que tarda el backend en procesar la consulta.
        var stopwatch = Stopwatch.StartNew();

        // Consulta base del catalogo.
        var query = _context.EngineeringParts
            .AsNoTracking();

        query = PartSpecification.ApplyFilters(
            query,
            filters);

        query = query.OrderBy(p => p.PartId);

        var countWatch = Stopwatch.StartNew();

        var totalElements = await query.LongCountAsync();

        countWatch.Stop();

        var dataWatch = Stopwatch.StartNew();

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

        dataWatch.Stop();

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