using EnrollmentService.Application.Repositories.Enrollments;
using EnrollmentService.Domain;
using EnrollmentService.Persistence.Context;
using Persistence.EntityFramework.Repositories;

namespace EnrollmentService.Persistence.Repositories.Enrollments;

internal class EnrollmentReadRepository(EnrollmentServiceDbContext dbContext) : ReadRepository<Enrollment>(dbContext), IEnrollmentReadRepository
{
}