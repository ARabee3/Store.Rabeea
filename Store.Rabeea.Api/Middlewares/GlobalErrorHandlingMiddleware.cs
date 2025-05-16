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
        }
        catch(Exception ex)
        {
            // Log Exception
            _logger.LogError(ex,ex.Message);

            // Response
            // 1. Set Status Code
            // 2. Set Content Type
            // 3. Set Response Object (Body)
            // 4. return response
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = ex.Message
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        
    }
}
