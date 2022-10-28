using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Behaviors;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
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

            // TokenService
            builder.RegisterType<TokenService>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // UnitOfWork
            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // IdentityHelper
            builder.RegisterType<IdentityHelper>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // AuthorizationHandlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<IAuthorizationHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();

            // Repositores
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();
        }
    }
}