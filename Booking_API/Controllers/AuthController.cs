﻿using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Auth;
using Booking_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ServiceResponse<int>> Register(UserRegisterDTO userRegisterDTO)
        {
            return await _authService.Register(userRegisterDTO);
        }

        [HttpPost("Login")]
        public async Task<ServiceResponse<string>> Login(UserLoginDTO userLoginDTO)
        {
            return await _authService.Login(userLoginDTO);
        }

      
    }
}
