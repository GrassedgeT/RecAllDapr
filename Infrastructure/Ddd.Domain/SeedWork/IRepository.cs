namespace ClassLibrary1.SeedWork;

public interface IRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot {
    IUnitOfWork UnitOfWork { get; }
}