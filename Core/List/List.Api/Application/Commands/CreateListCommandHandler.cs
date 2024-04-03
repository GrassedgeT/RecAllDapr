using MediatR;
using RecAll.Core.List.Api.Infrastructure.Services;
using RecAll.Core.List.Domain.AggregateModels;
using TheSalLab.GeneralReturnValues;

namespace RecAll.Core.List.Api.Application.Commands;

public class CreateListCommandHandler : IRequestHandler<CreateListCommand, ServiceResult>
{
    private readonly IIdentityService _identityService;
    private readonly IListRepository _listRepository;
    
    public CreateListCommandHandler(IIdentityService identityService, IListRepository listRepository)
    {
        _identityService = identityService;
        _listRepository = listRepository;
    }
    
    public async Task<ServiceResult> Handle(CreateListCommand command, CancellationToken cancellationToken)
    {
        var list = new Domain.AggregateModels.List(command.Name, command.TypeId, _identityService.GetUserIdentityGuid());
        
        _listRepository.Add(list);
        
        return await _listRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken)
            ? ServiceResult.CreateSucceededResult() 
            : ServiceResult.CreateFailedResult();
   }
}