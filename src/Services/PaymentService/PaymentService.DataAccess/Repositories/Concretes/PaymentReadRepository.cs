using PaymentService.DataAccess.Context;
using PaymentService.DataAccess.Repositories.Abstractions;
using PaymentService.Entities.Models;
using Persistence.EntityFramework.Repositories;

namespace PaymentService.DataAccess.Repositories.Concretes;

internal class PaymentReadRepository : ReadRepository<Payment>, IPaymentReadRepository
{
    public PaymentReadRepository(PaymentServiceDbContext context) : base(context)
    {
    }
}
