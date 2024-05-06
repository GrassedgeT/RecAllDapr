using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecAll.Contrib.MaskedTextItem.Api.Commands;
using RecAll.Contrib.MaskedTextItem.Api.Services;
using TheSalLab.GeneralReturnValues;

namespace RecAll.Contrib.MaskedTextItem.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ItemController
{
    private readonly MaskedTextItemContext _maskedTextItemContext;
    private readonly IIdentityService _identityService;
    private readonly ILogger<ItemController> _logger;
    
    public ItemController(MaskedTextItemContext maskedTextItemContext,
        IIdentityService identityService, ILogger<ItemController> logger)
    {
        _maskedTextItemContext = maskedTextItemContext;
        _identityService = identityService;
        _logger = logger;
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<ActionResult<ServiceResultViewModel<string>>> CreateAsync(
        [FromBody] CreateMaskedTextItemCommand command)
    {
        _logger.LogInformation(
            "----- Handling command {CommandName} ({@Command})",
            command.GetType().Name, command
            );
        var maskedTextItem = new Models.MaskedTextItem
        {
            Content = command.Content,
            MaskedContent = command.MaskedContent,
            UserIdentityGuid = _identityService.GetUserIdentityGuid(),
            IsDeleted = false
        };
        var maskedTextItemEntity = _maskedTextItemContext.Add(maskedTextItem);
        await _maskedTextItemContext.SaveChangesAsync();
        
        _logger.LogInformation("----- Command {CommandName} handled",
            command.GetType().Name
            );
        
        return ServiceResult<string>
            .CreateSucceededResult(maskedTextItemEntity.Entity.Id.ToString())
            .ToServiceResultViewModel();
    }
}