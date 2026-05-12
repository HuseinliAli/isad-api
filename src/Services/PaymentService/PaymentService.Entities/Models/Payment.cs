using Core.Domain.Entities;
using Core.Shared.Models;
using PaymentService.Entities.Enums;

namespace PaymentService.Entities.Models;

public class Payment : AuditableEntity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = Currency.EUR;
    public string CourseId { get; set; } = default!;
    public string CourseName { get; set; } = default!;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CreditCard;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

}