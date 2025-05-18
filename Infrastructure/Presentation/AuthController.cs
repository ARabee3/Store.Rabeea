using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager serviceManager) : ControllerBase
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(response);
        }
        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var response = await _serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(response);
        }
    }
}
