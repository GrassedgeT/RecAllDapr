namespace RecAll.Infrastructure.EventBus.Events;

public interface
    IIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent {
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler { }