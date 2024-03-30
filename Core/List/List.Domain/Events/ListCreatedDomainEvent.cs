using MediatR;

namespace RecAll.Core.List.Domain.Events;

public class ListCreatedDomainEvent : INotification {
    public AggregateModels.List List { get; set; }

    public ListCreatedDomainEvent(AggregateModels.List list) {
        List = list;
    }
}