using Bogus;
using CourseService.Domain;

namespace CourseService.Persistence.Context;

public static class DatabaseSeeder
{
    public static void Seed(CourseServiceDbContext context)
    {
        SeedTopics(context);
        SeedCourses(context);
        SeedPromisedSkills(context);
        SeedUnits(context);
        SeedLessons(context);
    }

    private static void SeedTopics(CourseServiceDbContext context)
    {
        var topicNames = new[]
        {
            "Web Development", "Mobile Development", "Data Science",
            "Machine Learning", "DevOps", "Cybersecurity",
            "Cloud Computing", "Blockchain", "Game Development", "UI/UX Design"
        };

        var existingNames = context.Set<Topic>()
            .Select(t => t.Name)
            .ToHashSet();

        var topics = topicNames
            .Where(name => !existingNames.Contains(name))
            .OrderBy(_ => Guid.NewGuid())
            .Take(5)
            .Select(name => new Topic { Id = Guid.NewGuid(), Name = name })
            .ToList();

        if (!topics.Any()) return;

        context.Set<Topic>().AddRange(topics);
        context.SaveChanges();
    }

    private static void SeedCourses(CourseServiceDbContext context)
    {
        if (context.Set<Course>().Any()) return;

        var topicIds = context.Set<Topic>().Select(t => t.Id).ToList();

        var faker = new Faker<Course>()
            .RuleFor(c => c.Id, _ => Guid.NewGuid())
            .RuleFor(c => c.Name, f => $"{f.Hacker.Adjective()} {f.Hacker.Noun()} Masterclass")
            .RuleFor(c => c.Description, f => f.Lorem.Paragraphs(2))
            .RuleFor(c => c.Price, f => Math.Round(f.Random.Decimal(9.99m, 199.99m), 2))
            .RuleFor(c => c.Level, f => f.PickRandom<CourseLevel>())
            .RuleFor(c => c.IsPublished, f => f.Random.Bool(0.7f))
            .RuleFor(c => c.TopicId, f => f.PickRandom(topicIds));

        var courses = faker.Generate(10);
        context.Set<Course>().AddRange(courses);
        context.SaveChanges();
    }

    private static void SeedPromisedSkills(CourseServiceDbContext context)
    {
        if (context.Set<PromisedSkill>().Any()) return;

        var courseIds = context.Set<Course>().Select(c => c.Id).ToList();
        var rng = new Faker();

        var skills = courseIds.SelectMany(courseId =>
        {
            return new Faker<PromisedSkill>()
                .RuleFor(s => s.Id, _ => Guid.NewGuid())
                .RuleFor(s => s.Name, f => $"{f.Hacker.Verb()} {f.Hacker.Noun()}")
                .RuleFor(s => s.CourseId, _ => courseId)
                .Generate(rng.Random.Int(3, 6));
        }).ToList();

        context.Set<PromisedSkill>().AddRange(skills);
        context.SaveChanges();
    }

    private static void SeedUnits(CourseServiceDbContext context)
    {
        if (context.Set<Unit>().Any()) return;

        var courseIds = context.Set<Course>().Select(c => c.Id).ToList();
        var rng = new Faker();

        var units = courseIds.SelectMany(courseId =>
        {
            var index = 0;
            return new Faker<Unit>()
                .RuleFor(u => u.Id, _ => Guid.NewGuid())
                .RuleFor(u => u.Name, f => $"Unit {++index}: {f.Lorem.Sentence(3)}")
                .RuleFor(u => u.IsPublished, f => f.Random.Bool(0.8f))
                .RuleFor(u => u.CourseId, _ => courseId)
                .Generate(rng.Random.Int(3, 5));
        }).ToList();

        context.Set<Unit>().AddRange(units);
        context.SaveChanges();
    }

    private static void SeedLessons(CourseServiceDbContext context)
    {
        if (context.Set<Lesson>().Any()) return;

        var unitIds = context.Set<Unit>().Select(u => u.Id).ToList();
        var rng = new Faker();

        var lessons = unitIds.SelectMany(unitId =>
        {
            return new Faker<Lesson>()
                .RuleFor(l => l.Id, _ => Guid.NewGuid())
                .RuleFor(l => l.Name, f => f.Lorem.Sentence(4))
                .RuleFor(l => l.Content, f => f.Lorem.Paragraphs(3))
                .RuleFor(l => l.IsPublished, f => f.Random.Bool(0.75f))
                .RuleFor(l => l.UnitId, _ => unitId)
                .Generate(rng.Random.Int(4, 8));
        }).ToList();

        context.Set<Lesson>().AddRange(lessons);
        context.SaveChanges();
    }
}