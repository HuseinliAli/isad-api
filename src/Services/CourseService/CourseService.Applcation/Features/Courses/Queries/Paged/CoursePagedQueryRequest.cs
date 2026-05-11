using CourseService.Applcation.Repositories;
using CourseService.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Base.Models;
using Persistence.EntityFramework.Extensions;

namespace CourseService.Applcation.Features.Courses.Queries.Paged;

public record CoursePagedQueryResponse
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public decimal Price { get; init; }
    public CourseLevel Level { get; init; }

    public string? TopicName { get; init; }

    public int UnitCount { get; init; }
    public int LessonCount { get; init; }

    public List<string>? PromisedSkills { get; init; }
}
public record CoursePagedQueryRequest : RequestParameters, IRequest<OffSetPagedList<CoursePagedQueryResponse>>;

public class CoursePagedQueryRequestHandler(IRepositoryManager repositoryManager) : IRequestHandler<CoursePagedQueryRequest, OffSetPagedList<CoursePagedQueryResponse>>
{
    public async Task<OffSetPagedList<CoursePagedQueryResponse>> Handle(
    CoursePagedQueryRequest request,
    CancellationToken cancellationToken)
    {
        var query = repositoryManager.CourseRead
            .FindMany(c => !c.IsPublished)
            .Include(c => c.Topic)
            .Include(c => c.Units)
                .ThenInclude(u => u.Lessons)
            .Include(c => c.PromisedSkills)
            .Select(c => new CoursePagedQueryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Level = c.Level,
                TopicName = c.Topic.Name,
                UnitCount = c.Units.Count,
                LessonCount = c.Units.SelectMany(u => u.Lessons).Count(),
                PromisedSkills = c.PromisedSkills.Select(s => s.Name).ToList()
            });

        return await query.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
