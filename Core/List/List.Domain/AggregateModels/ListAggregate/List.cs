using ClassLibrary1.SeedWork;
using RecAll.Core.List.Domain.Events;

namespace RecAll.Core.List.Domain.AggregateModels;

public class List : Entity, IAggregateRoot
{
    private string _name;
    
    private int _typeId;
    
    public ListType Type { get; private set; }

    private string _userIdentityGuid;
    
    public string UserIdentityGuid => _userIdentityGuid;
    
    private bool _isDeleted;
    
    public bool IsDeleted => _isDeleted;
    
    private List() { }
    
    public List(string name, int typeId, string userIdentityGuid) : this() {
        _name = name;
        _typeId = typeId;
        _userIdentityGuid = userIdentityGuid;
        
        var listCreatedDomainEvent = new ListCreatedDomainEvent(this);
        AddDomainEvent(listCreatedDomainEvent);
    }
    
    public void SetDeleted() {
        if (_isDeleted) {
            ThrowDeletedException();
        }

        _isDeleted = true;
        
        var listDeletedDomainEvent = new ListDeletedDomainEvent(this);
        AddDomainEvent(listDeletedDomainEvent);
    }
    
    private void ThrowDeletedException() =>
        throw new ListDomainException("列表已删除。");
    
    public void SetName(string name) {
        if (_isDeleted) {
            ThrowDeletedException();
        }

        _name = name;
    }
}