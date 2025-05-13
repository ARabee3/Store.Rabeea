using AutoMapper;
using AutoMapper.Execution;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class PictureUrlResolver(IHttpContextAccessor httpContextAccessor) : IValueResolver<Product, ProductResultDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;
            return $"{request.Scheme}://{request.Host}/{source.PictureUrl}";
        }
    }
}
