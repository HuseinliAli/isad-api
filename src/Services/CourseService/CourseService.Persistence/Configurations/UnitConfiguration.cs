using CourseService.Domain;
using CourseService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Persistence.Configurations;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {

        builder.ToTable("Units", CourseServiceDbContext.DEFAULT_SCHEMA);
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.CourseId)
            .IsRequired();

        builder.HasOne(u => u.Course)
            .WithMany(c => c.Units)
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Lessons)
            .WithOne(l => l.Unit)
            .HasForeignKey(l => l.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => u.CourseId);
        builder.HasIndex(u => u.IsPublished);
    }
}