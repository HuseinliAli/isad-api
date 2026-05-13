using EnrollmentService.Domain.Snapshots;
using EnrollmentService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnrollmentService.Persistence.Configurations;

public class CourseSnapshotConfiguration : IEntityTypeConfiguration<CourseSnapshot>
{
    public void Configure(EntityTypeBuilder<CourseSnapshot> builder)
    {
        builder.ToTable("CourseSnapshots", EnrollmentServiceDbContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.CourseId);

        builder.Property(x => x.CourseId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);


        builder.OwnsMany(x => x.Units, unit =>
        {
            unit.ToTable("UnitSnapshots", EnrollmentServiceDbContext.DEFAULT_SCHEMA);

            unit.WithOwner()
                .HasForeignKey("CourseId");

            unit.HasKey(x => x.UnitId);

            unit.Property(x => x.UnitId)
                .ValueGeneratedNever();

            unit.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
            unit.OwnsMany(x => x.Lessons, lesson =>
            {
                lesson.ToTable("LessonSnapshots", EnrollmentServiceDbContext.DEFAULT_SCHEMA);

                lesson.WithOwner()
                    .HasForeignKey("UnitId");

                lesson.HasKey(x => x.LessonId);

                lesson.Property(x => x.LessonId)
                    .ValueGeneratedNever();

                lesson.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        });
    }
}
//public class LessonSnapshotConfiguration : IEntityTypeConfiguration<LessonSnapshot>
//{
//    public void Configure(EntityTypeBuilder<LessonSnapshot> builder)
//    {
//        //builder.ToTable("LessonSnapshots", EnrollmentServiceDbContext.DEFAULT_SCHEMA);

//        //builder.HasKey(x => x.LessonId);

//        //builder.Property(x => x.Name)
//        //    .IsRequired()
//        //    .HasMaxLength(200);
//    }
//}


//public class UnitSnapshotConfiguration : IEntityTypeConfiguration<UnitSnapshot>
//{
//    public void Configure(EntityTypeBuilder<UnitSnapshot> builder)
//    {
//        //builder.ToTable("UnitSnapshots", EnrollmentServiceDbContext.DEFAULT_SCHEMA);

//        //builder.HasKey(x => x.UnitId);

//        //builder.Property(x => x.Name)
//        //    .IsRequired()
//        //    .HasMaxLength(200);

//        //builder.HasMany(x => x.Lessons)
//        //    .WithOne()
//        //    .HasForeignKey("UnitId")
//        //    .OnDelete(DeleteBehavior.Cascade);
//    }
//}
