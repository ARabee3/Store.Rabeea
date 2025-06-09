using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Identity;
using Services;
using Shared;
using Store.Rabeea.Api.ErrorsModels;
using Store.Rabeea.Api.Middlewares;
using System.Text;
namespace Store.Rabeea.Api.Extensions;

public static class Extensions
{
    public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddBuiltInServices();
        services.AddIdentityServices();
        services.AddSwaggerServices();

        services.ConfigureJwtServices(configuration);   
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
        app.UseAuthentication();
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
    private static IServiceCollection ConfigureJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        ).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))

            };
        });

        return services;
    }
    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<StoreIdentityDbContext>();
        return services;
    }
    private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Define the security scheme (JWT Bearer)
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            // Add a security requirement to use the defined scheme
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });
        });

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
        await dbInitializer.InitializeIdentityAsync();
        return app;
    }
    private static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        return app;
    }

}
