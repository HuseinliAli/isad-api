using PaymentService.DataAccess.Context;
using PaymentService.DataAccess.Repositories.Abstractions;
using PaymentService.Entities.Models;
using Persistence.EntityFramework.Repositories;

namespace PaymentService.DataAccess.Repositories.Concretes;

internal class PaymentWriteRepository : WriteRepository<Payment>, IPaymentWriteRepository
{
    public PaymentWriteRepository(PaymentServiceDbContext context) : base(context)
    {
    }
}