
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using Store.Rabeea.Api.ErrorsModels;
using Store.Rabeea.Api.Middlewares;
using System.Collections.Concurrent;

namespace Store.Rabeea.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
       {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager,ServiceManager>();
            builder.Services.AddScoped<IServiceManager,ServiceManager>();
            builder.Services.AddScoped<ConcurrentDictionary<string, object>>();
            builder.Services.AddAutoMapper(typeof(AssemblyReference));
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<ApiBehaviorOptions>(config =>
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




            var app = builder.Build();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            #region seeding
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthorization();
            app.UseStaticFiles();
            
            app.MapControllers();

            app.Run();
        }
    }
}
