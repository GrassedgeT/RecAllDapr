namespace ClassLibrary1.SeedWork;

public interface IUnitOfWork : IDisposable {
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}