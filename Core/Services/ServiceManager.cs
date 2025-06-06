﻿using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, ICacheRepository cacheRepository,UserManager<AppUser> userManager,IConfiguration configuration) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository,mapper );
        public ICacheService CacheService { get; } = new CacheService(cacheRepository);
        public IAuthService AuthService { get; set; } = new AuthService(userManager,configuration);
    }
}
