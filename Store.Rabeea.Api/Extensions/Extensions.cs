using Microsoft.AspNetCore.Mvc;
using Services;
using Persistence;
using Store.Rabeea.Api.ErrorsModels;
using Domain.Contracts;
using Store.Rabeea.Api.Middlewares;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
namespace Store.Rabeea.Api.Extensions;

public static class Extensions
{
    public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddBuiltInServices();
        services.AddSwaggerServices();
      
        services.AddInfraStructureServices(configuration);
        services.AddApplicationServices();

        services.ConfigureServices();
       
        return services;
    }
    public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
    {
        await app.InitializeDatabaseAsync();
        app.UseGlobalErrorHandling();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapControllers();

        return app;
    }

    private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();

        return services;
    }
    private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(config =>
           config.InvalidModelStateResponseFactory = (actionContext) =>
           {
               var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                       .Select(m => new ValidationError()
                       {
                           Field = m.Key,
                           Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                       });
               var response = new ValidationErrorResponse()
               {
                   Errors = errors
               };
               return new BadRequestObjectResult(response);
           }

       );
        return services;
    }
    private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.InitializeAsync();
        return app;
    }
    private static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        return app;
    }

}
