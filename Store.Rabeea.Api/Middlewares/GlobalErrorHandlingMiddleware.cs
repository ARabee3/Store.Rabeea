using Domain.Exceptions;
using Store.Rabeea.Api.ErrorsModels;

namespace Store.Rabeea.Api.Middlewares;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

    public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try {
            await _next.Invoke(context);
            // Catch the 404 not found for the path
            if(context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                await HandlingNotFoundEndpointAsync(context);
            }
        }
        catch(Exception ex)
        {
            // Log Exception
            _logger.LogError(ex, ex.Message);

            await HandlingErrorAsync(context, ex);
        }

    }

    private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
    {
        // Response
        // 1. Set Status Code
        // 2. Set Content Type
        // 3. Set Response Object (Body)
        // 4. return response
        context.Response.ContentType = "application/json";

        var response = new ErrorDetails()
        {
            ErrorMessage = ex.Message
        };
        response.StatusCode = ex switch
        {
            NotFoundException => 404,
            _ => 500
        };
        context.Response.StatusCode = response.StatusCode;

        await context.Response.WriteAsJsonAsync(response);
    }

    private static async Task HandlingNotFoundEndpointAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorDetails()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorMessage = $"Endpoint {context.Request.Path} is not found"
        };
        await context.Response.WriteAsJsonAsync(response);
    }
}
