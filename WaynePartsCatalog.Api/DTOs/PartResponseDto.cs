namespace WaynePartsCatalog.Api.DTOs;

// DTO que representa la información de una parte devuelta al cliente.
public class PartResponseDto
{
    public Guid PartId { get; set; }

    public DateOnly ManufactureDate { get; set; }

    public DateTime RegistrationTimestamp { get; set; }

    public int WeightKg { get; set; }

    public decimal SizeMeters { get; set; }

    public string PartType { get; set; } = string.Empty;

    public string Material { get; set; } = string.Empty;

    public string LongDescription { get; set; } = string.Empty;
}