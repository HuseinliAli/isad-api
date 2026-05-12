using Persistence.Base.Abstractions;

namespace PaymentService.DataAccess.Repositories.Abstractions;

public interface IRepositoryManager
{
    public IPaymentReadRepository PaymentRead { get; }
    public IPaymentWriteRepository PaymentWrite { get; }
    public IUnitOfWork UnitOfWork { get; }
}