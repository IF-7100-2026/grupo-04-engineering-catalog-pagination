using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaynePartsCatalog.Api.Models;

// Modelo que representa una parte del catalogo almacenada en PostgreSQL.
public class EngineeringPart
{
    [Key]
    [Column("part_id")]
    public Guid PartId { get; set; }

    [Required]
    [Column("manufacture_date")]
    public DateOnly ManufactureDate { get; set; }

    [Required]
    [Column("registration_timestamp")]
    public DateTime RegistrationTimestamp { get; set; }

    [Required]
    [Column("weight_kg")]
    public int WeightKg { get; set; }

    [Required]
    [Column("size_meters")]
    public decimal SizeMeters { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("part_type")]
    public string PartType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("material")]
    public string Material { get; set; } = string.Empty;

    [Required]
    [Column("long_description")]
    public string LongDescription { get; set; } = string.Empty;
}