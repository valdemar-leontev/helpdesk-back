namespace Helpdesk.Domain.Contracts;

public abstract class DictionaryBaseEntity : IEntity
{
    public int Id { get; set; }

    public required string Description { get; set; }
}