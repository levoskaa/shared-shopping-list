using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add secrets to configuration object
builder.Host.ConfigureAppConfiguration((context, config) =>
    config.AddJsonFile("secrets.json", false, true));

#region Add services to the container.
// Autofac configuration
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        // AutoMapper
        builder.RegisterAutoMapper(typeof(Program).Assembly);
        // Custom types
        builder.RegisterModule(new SharedShoppingListModule());
    });

// Controllers
builder.Services.AddControllers();

// Swagger - more about configuration at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<SharedShoppingListContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
