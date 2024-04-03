using System.Reflection;
using Autofac;
using MediatR;
using RecAll.Core.List.Api.Application.Behaviors;
using RecAll.Core.List.Api.Application.Commands;
using Module = Autofac.Module;

namespace RecAll.Core.List.Api.Infrastructure.AutofacModules;

public class MediatorModule : Module {
    protected override void Load(ContainerBuilder builder) {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(CreateListCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        builder.RegisterGeneric(typeof(LoggingBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
    }
}