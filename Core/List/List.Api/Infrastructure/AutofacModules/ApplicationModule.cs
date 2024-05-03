using System.Data.Common;
using Autofac;
using RecAll.Core.List.Api.Application.IntegrationEvents;
using RecAll.Core.List.Api.Infrastructure.Services;
using RecAll.Core.List.Domain.AggregateModels;
using RecAll.Core.List.Domain.AggregateModels.ItemAggregate;
using RecAll.Core.List.Domain.AggregateModels.SetAggregate;
using RecAll.Core.List.Infrastructure.Repositories;
using RecAll.Infrastructure.EventBus;
using RecAll.Infrastructure.EventBus.Abstractions;
using RecAll.Infrastructure.IntegrationEventLog.Services;

namespace RecAll.Core.List.Api.Infrastructure.AutofacModules;

public class ApplicationModule : Module {
    protected override void Load(ContainerBuilder builder) {
        builder.RegisterType<ListRepository>().As<IListRepository>()
            .InstancePerLifetimeScope();
        builder.RegisterType<IdentityService>().As<IIdentityService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<SetRepository>().As<ISetRepository>()
            .InstancePerLifetimeScope();
        builder.RegisterType<ContribUrlService>().As<IContribUrlService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<ItemRepository>().As<IItemRepository>()
            .InstancePerLifetimeScope();
        builder.Register<Func<DbConnection, IIntegrationEventLogService>>(_ =>
            connection => new IntegrationEventLogService(connection));
        builder.RegisterType<ListIntegrationEventService>()
            .As<IListIntegrationEventService>().InstancePerLifetimeScope();
        builder.RegisterType<DaprEventBus>().As<IEventBus>()
            .InstancePerLifetimeScope();
    }
}