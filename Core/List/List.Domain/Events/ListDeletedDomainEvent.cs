using MediatR;

namespace RecAll.Core.List.Domain.Events;

public class ListDeletedDomainEvent : INotification {
    public AggregateModels.List List { get; }

    public ListDeletedDomainEvent(AggregateModels.List list) {
        List = list;
    }
}