using PaymentService.Entities.Models;
using Persistence.Base.Abstractions;

namespace PaymentService.DataAccess.Repositories.Abstractions;

public interface IPaymentReadRepository : IReadRepository<Payment>
{
}