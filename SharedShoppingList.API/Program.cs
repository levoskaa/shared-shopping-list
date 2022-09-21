using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Data;

var builder = WebApplication.CreateBuilder(args);
// Add secrets to configuration object
builder.Host.ConfigureAppConfiguration((context, config) =>
    config.AddJsonFile("secrets.json", false, true));

#region Add services to the container.
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
