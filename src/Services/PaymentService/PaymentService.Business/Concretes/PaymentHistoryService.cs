using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using PaymentService.Business.Abstractions;
using PaymentService.DataAccess.Repositories.Abstractions;
using PaymentService.Entities.Models;

namespace PaymentService.Business.Concretes;

internal class PaymentHistoryService(IRepositoryManager repositoryManager, ICurrentUserService currentUserService) : IPaymentHistoryService
{
    public async Task<IEnumerable<PaymentHistoryRecord>> PaymentHistoryAsync()
    {
        int currentUserId = currentUserService.UserId;

        var paymentHistory = await repositoryManager.PaymentRead.FindMany(ph => ph.CreatedBy == currentUserId.ToString()).ToListAsync();
        var result = paymentHistory.Select(ph => new PaymentHistoryRecord
        {
            Id = ph.Id,
            PaymentId = ph.Id,
            Timestamp = ph.CreatedAt,
            Status = ph.Status switch
            {
                PaymentStatus.Pending => "Pending",
                PaymentStatus.Success => "Success",
                PaymentStatus.Failed => "Failed",
                _ => "Unknown"
            },
            Amount = ph.Amount,
            CourseName = ph.CourseName,
        });

        return result;

    }
}
