using Core.Domain.Exceptions;
using EnrollmentService.Application.Repositories;
using EnrollmentService.Domain;
using EnrollmentService.Domain.Events.IntegrationEvents;
using EventBus.Base.Abstractions;
using Infrastructure.Services.Abstractions;
using MediatR;

namespace EnrollmentService.Application.Features.Enrollments.Commands.Create;

public class EnrollmentCreateCommandRequestHandler(IRepositoryManager repositoryManager, ICurrentUserService currentUserService, IEventBus eventBus) : IRequestHandler<EnrollmentCreateCommandRequest>
{
    public async Task Handle(EnrollmentCreateCommandRequest request, CancellationToken cancellationToken)
    {
        //var courseSnapshot = await repositoryManager.CourseSnapshotRead.FindOneAsync(cs => cs.CourseId == request.CourseId.ToString());
        //if (courseSnapshot == null)
        //    throw new BadRequestException("Course is not exists");
        var isEnrolledBefore = await repositoryManager.EnrollmentRead.AnyAsync(e => e.CourseId == request.CourseId && e.UserId == currentUserService.UserId);
        if (isEnrolledBefore)
            throw new BadRequestException("You already registered to this course");
        
        Enrollment enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            UserId = currentUserService.UserId
            
        };

        await repositoryManager.EnrollmentWrite.AddAsync(enrollment);
        await repositoryManager.UnitOfWork.SaveChangesAsync();

        await eventBus.Publish(new CourseEnrolledIntegrationEvent
        {
            CourseId = enrollment.CourseId,
            CourseName = "Test",
            UserId = enrollment.UserId,
            Amount = 10.0m
        });
    }
}