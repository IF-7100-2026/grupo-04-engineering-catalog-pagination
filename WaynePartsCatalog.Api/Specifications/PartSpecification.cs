using WaynePartsCatalog.Api.DTOs;
using WaynePartsCatalog.Api.Models;

namespace WaynePartsCatalog.Api.Specifications;

// Clase encargada de aplicar filtros dinámicos sobre la consulta de partes.
// Permite construir consultas LINQ de forma modular.
public static class PartSpecification
{

    // Aplica filtros opcionales sobre la consulta de partes.
    // Cada filtro solo se aplica si el valor está presente.
    public static IQueryable<EngineeringPart> ApplyFilters(
        IQueryable<EngineeringPart> query,
        PartFilterDto filters)
    {
        if (filters.ManufactureDateFrom.HasValue)
        {
            query = query.Where(p =>
                p.ManufactureDate >= filters.ManufactureDateFrom.Value);
        }

        if (filters.ManufactureDateTo.HasValue)
        {
            query = query.Where(p =>
                p.ManufactureDate <= filters.ManufactureDateTo.Value);
        }

        if (filters.RegistrationFrom.HasValue)
        {
            query = query.Where(p =>
                p.RegistrationTimestamp >= filters.RegistrationFrom.Value);
        }

        if (filters.RegistrationTo.HasValue)
        {
            query = query.Where(p =>
                p.RegistrationTimestamp <= filters.RegistrationTo.Value);
        }

        if (filters.WeightFrom.HasValue)
        {
            query = query.Where(p =>
                p.WeightKg >= filters.WeightFrom.Value);
        }

        if (filters.WeightTo.HasValue)
        {
            query = query.Where(p =>
                p.WeightKg <= filters.WeightTo.Value);
        }

        if (filters.SizeFrom.HasValue)
        {
            query = query.Where(p =>
                p.SizeMeters >= filters.SizeFrom.Value);
        }

        if (filters.SizeTo.HasValue)
        {
            query = query.Where(p =>
                p.SizeMeters <= filters.SizeTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(filters.Material))
        {
            query = query.Where(p =>
                p.Material == filters.Material);
        }

        if (!string.IsNullOrWhiteSpace(filters.PartType))
        {
            query = query.Where(p =>
                p.PartType == filters.PartType);
        }

        if (!string.IsNullOrWhiteSpace(filters.DescriptionContains))
        {
            query = query.Where(p =>
                p.LongDescription.Contains(filters.DescriptionContains));
        }

        return query;
    }
}