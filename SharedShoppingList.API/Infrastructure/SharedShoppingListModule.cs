using Autofac;
using FluentValidation;
using MediatR;
using SharedShoppingList.API.Application.Behaviors;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Services;
using System.Reflection;

namespace SharedShoppingList.API.Infrastructure
{
    public class SharedShoppingListModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Validators
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(type => type.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // Behaviors
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerDependency();

            // Dapper
            builder.RegisterType<DapperContext>()
                .AsSelf()
                .SingleInstance();

            // TokenGenerator
            builder.RegisterType<TokenGenerator>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // UnitOfWork
            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}