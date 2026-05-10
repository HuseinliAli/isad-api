namespace Core.Domain.Entities;

public interface IDraftableEntity : IEntity
{
    public bool IsPublished { get; set; }
}