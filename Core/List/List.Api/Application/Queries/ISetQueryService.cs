using RecAll.Core.List.Api.Application.Queries;

namespace RecAll.Core.List.Api.Infrastructure.Services;

public interface ISetQueryService {
    Task<(IEnumerable<SetViewModel>, int)> ListAsync(int listId, int skip,
        int take, string userIdentityGuid);

    Task<SetViewModel> GetAsync(int id, string userIdentityGuid);
}