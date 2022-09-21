using Autofac;
using SharedShoppingList.API.Data;

namespace SharedShoppingList.API.Infrastructure
{
    public class SharedShoppingListModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Dapper
            builder.RegisterType<DapperContext>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
