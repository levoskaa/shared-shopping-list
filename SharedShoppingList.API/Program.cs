using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Infrastructure;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add secrets to configuration object
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config.Sources.Clear();
    var env = context.HostingEnvironment;

    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

    if (env.IsDevelopment())
    {
        if (!string.IsNullOrEmpty(env.ApplicationName))
        {
            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
            if (appAssembly != null)
            {
                config.AddUserSecrets(appAssembly, optional: true);
            }
        }
    }

    config.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

    config.AddEnvironmentVariables();

    if (args != null)
    {
        config.AddCommandLine(args);
    }
});

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
builder.Services.AddControllers(options =>
{
    // Require authentication by default on all endpoints
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

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
            Array.Empty<string>()
        }
    });
});

// DbContext
builder.Services.AddDbContext<SharedShoppingListContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("Default")));

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

// HealthChecks
builder.Services.AddHealthChecks();

// CORS
const string devCorsPolicy = "AllowDevOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: devCorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
#endregion

var app = builder.Build();

#region Apply migrations on startup
using (var scope = app.Services.CreateAsyncScope())
using (var dbContext = scope.ServiceProvider.GetService<SharedShoppingListContext>())
{
        await dbContext.Database.MigrateAsync();
}
#endregion

#region Seed initial data
using (var scope = app.Services.CreateAsyncScope())
using (var dbContext = scope.ServiceProvider.GetService<SharedShoppingListContext>())
{
    dbContext.Database.EnsureCreated();

    var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    await DbSeeder.SeedRolesAsync(roleManager);
    await DbSeeder.SeedUsers(userManager);

    await dbContext.SaveChangesAsync();
}
#endregion

#region Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(devCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
#endregion
