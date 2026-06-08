namespace WaynePartsCatalog.Api.DTOs;

public class PartFilterDto
{
    public DateOnly? ManufactureDateFrom { get; set; }

    public DateOnly? ManufactureDateTo { get; set; }

    public DateTimeOffset? RegistrationFrom { get; set; }

    public DateTimeOffset? RegistrationTo { get; set; }

    public int? WeightFrom { get; set; }

    public int? WeightTo { get; set; }

    public decimal? SizeFrom { get; set; }

    public decimal? SizeTo { get; set; }

    public string? Material { get; set; }

    public string? PartType { get; set; }

    public string? DescriptionContains { get; set; }
}