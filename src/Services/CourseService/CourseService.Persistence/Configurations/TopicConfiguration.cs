using CourseService.Domain;
using CourseService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Persistence.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topics", CourseServiceDbContext.DEFAULT_SCHEMA);

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.HasMany(t => t.Courses)
            .WithOne(c => c.Topic)
            .HasForeignKey(c => c.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}