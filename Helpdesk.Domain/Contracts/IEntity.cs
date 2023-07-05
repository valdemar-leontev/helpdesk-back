namespace Helpdesk.Domain.Contracts;

public interface IEntity<T>
{
    T Id { get; set; }
}

public interface IEntity : IEntity<int>
{
}