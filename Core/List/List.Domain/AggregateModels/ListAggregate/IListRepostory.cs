using ClassLibrary1.SeedWork;

namespace RecAll.Core.List.Domain.AggregateModels;

public interface IListRepository : IRepository<List> {
    List Add(List list);

    Task<List> GetAsync(int listId, string userIdentityGuid);
}