using Core.Domain.Exceptions;
using EnrollmentService.Application.Repositories;
using EnrollmentService.Domain;
using Infrastructure.Services.Abstractions;
using MediatR;

namespace EnrollmentService.Application.Features.Enrollments.Commands.Create;

public record EnrollmentCreateCommandRequest(Guid CourseId) : IRequest;

public class EnrollmentCreateCommandRequestHandler(IRepositoryManager repositoryManager, ICurrentUserService currentUserService) : IRequestHandler<EnrollmentCreateCommandRequest>
{
    public async Task Handle(EnrollmentCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if (!await repositoryManager.CourseSnapshotRead.AnyAsync(cs => cs.CourseId == request.CourseId.ToString())
            throw new BadRequestException("Course is not exists");

        Enrollment enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            UserId = currentUserService.UserId
        };

        await repositoryManager.EnrollmentWrite.AddAsync(enrollment);
        await repositoryManager.UnitOfWork.SaveChangesAsync();
    }
}