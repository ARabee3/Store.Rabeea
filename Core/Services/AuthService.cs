using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager) : IAuthService
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();
            var flag = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!flag) throw new UnauthorizedException();
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "Token"
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                throw new ValidationException(errors);
            }
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "Token"
            };
        }
    }
}
