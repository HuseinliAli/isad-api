using CourseService.Domain;
using CourseService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Persistence.Configurations;

public class PromisedSkillConfiguration : IEntityTypeConfiguration<PromisedSkill>
{
    public void Configure(EntityTypeBuilder<PromisedSkill> builder)
    {
        builder.ToTable("PromisedSkills", CourseServiceDbContext.DEFAULT_SCHEMA);

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
            .ValueGeneratedNever();

        builder.Property(ps => ps.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ps => ps.CourseId)
            .IsRequired();

        builder.HasOne(ps => ps.Course)
            .WithMany(c => c.PromisedSkills)
            .HasForeignKey(ps => ps.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ps => new { ps.CourseId, ps.Name })
            .IsUnique();
    }
}