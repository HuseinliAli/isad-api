using CourseService.Domain;
using CourseService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses", CourseServiceDbContext.DEFAULT_SCHEMA);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(c => c.Level)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.IsPublished)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.TopicId)
            .IsRequired();

        builder.HasOne(c => c.Topic)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.PromisedSkills)
            .WithOne(ps => ps.Course)
            .HasForeignKey(ps => ps.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Units)
            .WithOne(u => u.Course)
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.TopicId);
        builder.HasIndex(c => c.Level);
        builder.HasIndex(c => c.IsPublished);
    }
}