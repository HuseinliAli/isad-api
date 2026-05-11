using Core.Domain.Entities;

namespace CourseService.Domain;

public class Course : IDraftableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; }

    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }

    public ICollection<PromisedSkill> PromisedSkills { get; set; }
    public ICollection<Unit> Units { get; set; }
}
public class Topic : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; }
}

public class PromisedSkill : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; }

}

public class Lesson : IDraftableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }

    public Guid UnitId { get; set; }
    public Unit Unit { get; set; }

    public bool IsPublished { get; set; }
}

public class Unit : IDraftableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsPublished { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
}
