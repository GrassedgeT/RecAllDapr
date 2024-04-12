using RecAll.Infrastructure.EventBus.Events;

namespace RecAll.Core.List.Api.Infrastructure.Services;

public interface IListIntegrationEventService {
    Task AddAndSaveEventAsync(IntegrationEvent integrationEvent);

    Task PublishEventsAsync(Guid transactionId);
}