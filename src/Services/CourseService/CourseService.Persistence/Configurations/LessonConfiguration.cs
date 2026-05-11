using CourseService.Domain;
using CourseService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons", CourseServiceDbContext.DEFAULT_SCHEMA);

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Content)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(l => l.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(l => l.UnitId)
            .IsRequired();

        builder.HasOne(l => l.Unit)
            .WithMany(u => u.Lessons)
            .HasForeignKey(l => l.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => l.UnitId);
        builder.HasIndex(l => l.IsPublished);
    }
}