using EnrollmentService.Domain;
using EnrollmentService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnrollmentService.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments", EnrollmentServiceDbContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.EnrolledAt)
            .IsRequired();

        builder.Property(x => x.ProgressPercentage)
            .HasPrecision(5, 2)
            .HasDefaultValue(0);

        builder.HasMany(x => x.LessonProgresses)
            .WithOne()
            .HasForeignKey("EnrollmentId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}