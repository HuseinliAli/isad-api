using EnrollmentService.Application.Repositories.Enrollments;
using EnrollmentService.Domain;
using EnrollmentService.Persistence.Context;
using Persistence.EntityFramework.Repositories;

namespace EnrollmentService.Persistence.Repositories.Enrollments;

internal class EnrollmentWriteRepository(EnrollmentServiceDbContext dbContext) : WriteRepository<Enrollment>(dbContext), IEnrollmentWriteRepository
{
}
