using Microsoft.EntityFrameworkCore;
using WaynePartsCatalog.Api.Models;

namespace WaynePartsCatalog.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Representa la tabla principal del catalogo.
    public DbSet<EngineeringPart> EngineeringParts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeo entre el modelo EngineeringPart y la tabla catalog_parts.
        modelBuilder.Entity<EngineeringPart>(entity =>
        {
            entity.ToTable("catalog_parts");

            entity.HasKey(part => part.PartId);

            entity.Property(part => part.PartId)
                .HasColumnName("part_id");

            entity.Property(part => part.ManufactureDate)
                .HasColumnName("manufacture_date")
                .IsRequired();

            entity.Property(part => part.RegistrationTimestamp)
                .HasColumnName("registration_timestamp")
                .IsRequired();

            entity.Property(part => part.WeightKg)
                .HasColumnName("weight_kg")
                .IsRequired();

            entity.Property(part => part.SizeMeters)
                .HasColumnName("size_meters")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(part => part.PartType)
                .HasColumnName("part_type")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(part => part.Material)
                .HasColumnName("material")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(part => part.LongDescription)
                .HasColumnName("long_description")
                .IsRequired();
        });
    }
}