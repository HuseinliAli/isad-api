using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Business.Abstractions;

public interface IPaymentHistoryService
{
    Task<IEnumerable<PaymentHistoryRecord>> PaymentHistoryAsync();
}
public record PaymentHistoryRecord
{
    public Guid Id { get; init; }
    public Guid PaymentId { get; init; }
    public DateTime Timestamp { get; init; }
    public string Status { get; init; }
    public decimal Amount { get; init; }
    public string CourseName { get; init; } = string.Empty;
}