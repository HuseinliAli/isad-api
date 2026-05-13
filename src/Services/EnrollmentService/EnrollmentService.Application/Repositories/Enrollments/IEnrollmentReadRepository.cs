using EnrollmentService.Domain;
using Persistence.Base.Abstractions;

namespace EnrollmentService.Application.Repositories.Enrollments;

public interface IEnrollmentReadRepository : IReadRepository<Enrollment>
{
}
