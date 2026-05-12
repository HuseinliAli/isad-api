using EventBus.Base.Abstractions;
using EventBus.Base.Events;
using Microsoft.Extensions.Configuration;
using PaymentService.DataAccess.Repositories.Abstractions;
using PaymentService.Entities.Enums;
using PaymentService.Entities.Events;
using PaymentService.Entities.Models;

namespace PaymentService.Business.EventHandlers;

public class CourseEnrolledIntegrationEventHandler
    (IEventBus eventBus, IConfiguration configuration, IRepositoryManager repositoryManager): IIntegrationEventHandler<CourseEnrolledIntegrationEvent>
{
    public async Task Handle(CourseEnrolledIntegrationEvent @event)
    {
        string keyword = "PaymentSuccess";
        bool isSuccess = configuration.GetValue<bool>(keyword);

        IntegrationEvent paymentEvent = isSuccess
            ? new PaymentCompletedIntegrationEvent
            {
                CourseId = @event.CourseId,
                IsSuccess = true
            }
            : new PaymentFailedIntegrationEvent
            {
                CourseId = @event.CourseId,
                Reason = "Payment failed"
            };

        await repositoryManager.PaymentWrite.AddAsync(new Payment
        {
            CourseId = @event.CourseId,
            CourseName = @event.CourseName,
            Status = isSuccess ? PaymentStatus.Success : PaymentStatus.Failed,
            Amount = @event.Amount,
            Currency = Currency.EUR
        });
        await repositoryManager.UnitOfWork.SaveChangesAsync();
        await eventBus.Publish(paymentEvent);
  
    }
}
