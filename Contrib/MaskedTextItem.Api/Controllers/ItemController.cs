using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecAll.Contrib.MaskedTextItem.Api.Commands;
using RecAll.Contrib.MaskedTextItem.Api.Services;
using RecAll.Contrib.MaskedTextItem.Api.ViewModels;
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
    
    [Route("update")]
    [HttpPost]
    public async Task<ServiceResultViewModel> UpdateAsync(
        [FromBody] UpdateMaskedTextItemCommand command)
    {
        _logger.LogInformation(
            "----- Handling command {CommandName} ({@Command})",
            command.GetType().Name, command
            );
        var userIdentityGuid = _identityService.GetUserIdentityGuid();
        
        var maskedTextItem = await _maskedTextItemContext.MaskedTextItems
            .FirstOrDefaultAsync(p => p.Id == command.Id && userIdentityGuid == p.UserIdentityGuid && !p.IsDeleted);
        
        if (maskedTextItem == null)
        {
            _logger.LogWarning(
                $"用户{userIdentityGuid}尝试查看已删除、不存在或不属于自己的TextItem {command.Id}");

            return ServiceResult
                .CreateFailedResult($"Unknown MaskedTextItem id: {command.Id}")
                .ToServiceResultViewModel();
        }
        
        maskedTextItem.Content = command.Content;
        maskedTextItem.MaskedContent = command.MaskedContent;
        _maskedTextItemContext.Update(maskedTextItem);
        await _maskedTextItemContext.SaveChangesAsync();
        
        _logger.LogInformation("----- Command {CommandName} handled",
            command.GetType().Name
            );
        
        return ServiceResult.CreateSucceededResult()
            .ToServiceResultViewModel();
    }

    [Route("get/{id}")]
    [HttpGet]
    public async Task<ActionResult<ServiceResultViewModel<MaskedTextViewModel>>> GetAsync(int id)
    {
        _logger.LogInformation(
            "----- Handling command {CommandName} ({@Command})",
            "GetAsync itemId = ", id
            );
        var userIdentityGuid = _identityService.GetUserIdentityGuid();
        
        var maskedTextItem = await _maskedTextItemContext.MaskedTextItems
            .FirstOrDefaultAsync(p => p.Id == id && userIdentityGuid == p.UserIdentityGuid && !p.IsDeleted);
        if (maskedTextItem == null)
        {
            _logger.LogWarning(
                $"用户{userIdentityGuid}尝试查看已删除、不存在或不属于自己的TextItem {id}");

            return ServiceResult<MaskedTextViewModel>
                .CreateFailedResult($"Unknown MaskedTextItem id: {id}")
                .ToServiceResultViewModel();
        }
        
        _logger.LogInformation("----- Command {CommandName} handled",
            "GetAsync itemId = ", id
            );
        
        return ServiceResult<MaskedTextViewModel>
            .CreateSucceededResult(new MaskedTextViewModel
                {
                    Id = maskedTextItem.Id,
                    ItemId = maskedTextItem.ItemId,
                    Content = maskedTextItem.Content,
                    MaskedContent = maskedTextItem.MaskedContent  
                })
            .ToServiceResultViewModel();
    }

    [Route("getItems")]
    [HttpPost]
    public async Task<ActionResult<ServiceResultViewModel<IEnumerable<MaskedTextViewModel>>>> GetItemsAsync(GetItemsCommand command)
    {
        var itemids = command.Ids.ToList();
        _logger.LogInformation(
            "----- Handling command {CommandName} ({@Command})",
            command.GetType().Name, command,
            "ItemIds = ", itemids
            );
        var userIdentityGuid = _identityService.GetUserIdentityGuid();
        
        var maskedTextItems = await _maskedTextItemContext.MaskedTextItems
            .Where(p => userIdentityGuid == p.UserIdentityGuid && !p.IsDeleted && p.ItemId.HasValue && itemids.Contains(p.ItemId.Value))
            .ToListAsync();

        if (maskedTextItems.Count != itemids.Count)
        {
            var missingIds = string.Join(".", itemids.Except(maskedTextItems.Select(p => p.ItemId.Value)).Select(p => p.ToString()));
            
            _logger.LogWarning(
                $"用户{userIdentityGuid}尝试查看已删除、不存在或不属于自己的TextItem, ItemID: {missingIds}");
            return ServiceResult<IEnumerable<MaskedTextViewModel>>
                .CreateFailedResult($"Unknown MaskedTextItem with ItemID: {missingIds}")
                .ToServiceResultViewModel();
        }
        
        maskedTextItems.Sort((x,y) => 
            itemids.IndexOf(x.ItemId.Value) - itemids.IndexOf(y.ItemId.Value));
        
        _logger.LogInformation("----- Command {CommandName} handled",
            command.GetType().Name
            );
        return ServiceResult<IEnumerable<MaskedTextViewModel>>
            .CreateSucceededResult(maskedTextItems.Select(p => new MaskedTextViewModel
            {
                Id = p.Id, ItemId = p.ItemId, Content = p.Content, MaskedContent = p.MaskedContent
            })).ToServiceResultViewModel();
    }
    
    [Route("delete/{id}")]
    [HttpGet]
    public async Task<ServiceResultViewModel<string>> DeleteAsync(int id)
    {
        _logger.LogInformation(
            "----- Handling command {CommandName} ({@Command})",
            "DeleteAsync itemId = ", id
            );
        var userIdentityGuid = _identityService.GetUserIdentityGuid();
        
        var maskedTextItem = await _maskedTextItemContext.MaskedTextItems
            .FirstOrDefaultAsync(p => p.Id == id && userIdentityGuid == p.UserIdentityGuid && !p.IsDeleted);
        
        if (maskedTextItem == null)
        {
            _logger.LogWarning(
                $"用户{userIdentityGuid}尝试查看已删除、不存在或不属于自己的TextItem {id}");

            return ServiceResult<string>
                .CreateFailedResult($"Unknown MaskedTextItem id: {id}")
                .ToServiceResultViewModel();
        }
        
        maskedTextItem.IsDeleted = true;
        _maskedTextItemContext.Update(maskedTextItem);
        await _maskedTextItemContext.SaveChangesAsync();
        
        _logger.LogInformation("----- Command {CommandName} handled",
            "DeleteAsync itemId = ", id
            );
        
        return ServiceResult<string>
            .CreateSucceededResult(maskedTextItem.Id.ToString())
            .ToServiceResultViewModel();
    }
}