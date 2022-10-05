using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Infrastructure;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using System.Text;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using SharedShoppingList.API.Infrastructure.Authorization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add secrets to configuration object
builder.Host.ConfigureAppConfiguration((context, config) =>
    config.AddJsonFile("secrets.json", false, true));

#region Add services to the container.
// Autofac configuration
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        // AutoMapper
        builder.RegisterAutoMapper(Assembly.GetExecutingAssembly());
        // Custom types
        builder.RegisterModule(new SharedShoppingListModule());
    });

// Controllers
builder.Services.AddControllers();

// Swagger - more about configuration at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


// DbContext
builder.Services.AddDbContext<SharedShoppingListContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Default")));

// Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<SharedShoppingListContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false, // TODO: set to true and test
        ValidateAudience = false, // TODO: set to true and test
        ValidAudience = configuration["JWT:Audience"],
        ValidIssuer = configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MatchingUsername", policy => policy.Requirements.Add(new MatchingUsernameRequirement()));
    options.AddPolicy("UserGroupOwner", policy => policy.Requirements.Add(new UserGroupOwnerRequirement()));
});

// Don't let claim names to be overwritten
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
