using AutoMapper;
using Newtonsoft.Json;
using SharedShoppingList.API.Application.ViewModels;
using SharedShoppingList.API.Infrastructure.Exceptions;
using System.Net;

namespace SharedShoppingList.API.Infrastructure.ErrorHandling;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment env;
    private readonly IMapper mapper;

    public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, IMapper mapper)
    {
        this.next = next;
        this.env = env;
        this.mapper = mapper;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Call the next delegate/middleware in the pipeline.
            await next(httpContext);
        }
        catch (DomainException e)
        {
            await SendResponseAsync(
                httpContext,
                HttpStatusCode.BadRequest,
                mapper.Map<ErrorViewModel>(e));
        }
        catch (EntityNotFoundException e)
        {
            await SendResponseAsync(
                httpContext,
                HttpStatusCode.NotFound,
                mapper.Map<ErrorViewModel>(e));
        }
        catch (ForbiddenException e)
        {
            await SendResponseAsync(
                httpContext,
                HttpStatusCode.Forbidden,
                mapper.Map<ErrorViewModel>(e));
        }
        catch (UnauthorizedException e)
        {
            await SendResponseAsync(
                httpContext,
                HttpStatusCode.Unauthorized,
                mapper.Map<ErrorViewModel>(e));
        }
        catch (Exception e)
        {
            await SendResponseAsync(
                httpContext,
                HttpStatusCode.InternalServerError,
                mapper.Map<ErrorViewModel>(e));
        }
    }

    private async Task SendResponseAsync(HttpContext httpContext, HttpStatusCode statusCode, ErrorViewModel errorViewModel)
    {
        httpContext.Response.Clear();
        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/json";

        var json = JsonConvert.SerializeObject(
            errorViewModel,
            new JsonSerializerSettings
            {
                ContractResolver = new ErrorContractResolver(env.IsDevelopment()),
            });

        await httpContext.Response.WriteAsync(json);
    }
}