
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using Store.Rabeea.Api.ErrorsModels;
using Store.Rabeea.Api.Extensions;
using Store.Rabeea.Api.Middlewares;
using System.Collections.Concurrent;

namespace Store.Rabeea.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
