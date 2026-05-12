using Microsoft.Extensions.DependencyInjection;
using PaymentService.DataAccess.Repositories.Abstractions;
using Persistence.Base.Abstractions;

namespace PaymentService.DataAccess.Repositories.Concretes;

internal class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IPaymentWriteRepository> paymentWriteRepository;
    private readonly Lazy<IPaymentReadRepository> paymentReadRepository;    
    private readonly Lazy<IUnitOfWork> unitOfWork;

    public RepositoryManager(IServiceProvider sp)
    {
        paymentWriteRepository = new Lazy<IPaymentWriteRepository>(sp.GetRequiredService<IPaymentWriteRepository>);
        paymentReadRepository = new Lazy<IPaymentReadRepository>(sp.GetRequiredService<IPaymentReadRepository>);
        unitOfWork = new Lazy<IUnitOfWork>(sp.GetRequiredService<IUnitOfWork>);
    }

    public IPaymentWriteRepository PaymentWrite => paymentWriteRepository.Value;
    public IPaymentReadRepository PaymentRead => paymentReadRepository.Value;
    public IUnitOfWork UnitOfWork => unitOfWork.Value;
}