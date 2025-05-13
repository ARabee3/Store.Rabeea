
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.Abstractions;
using Services.MappingProfiles;
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
            var app = builder.Build();
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
