using MediatR;

namespace EnrollmentService.Application.Features.Enrollments.Commands.Create;

public record EnrollmentCreateCommandRequest(Guid CourseId) : IRequest;