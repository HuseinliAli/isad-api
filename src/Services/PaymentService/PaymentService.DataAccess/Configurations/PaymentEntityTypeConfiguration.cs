using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.DataAccess.Context;
using PaymentService.Entities.Models;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", PaymentServiceDbContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.Property(x => x.Currency)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .IsRequired();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.CourseName)
            .IsRequired()
            .HasMaxLength(200);
    }
}