using AbokiData.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbokiData.Models.DTOs.Requests;
using AbokiData.Models.DTOs.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AbokiCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AbokiAPI.Services;

namespace AbokiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly ILogger<AuthManagementController> _logger;
        private readonly IUserRepository _userRepository;

        public AuthManagementController(
             IUserRepository userRepository,
             ILogger<AuthManagementController> logger
            )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            //Utilize the model
            if (ModelState.IsValid)
            {
                var result = await _userRepository.RegisterUserAsync(user);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest("Some Properties are not valid");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {

            if (ModelState.IsValid)
            {
                var result = await _userRepository.LoginAsync(user);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            return BadRequest("some properties are not valid");

        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.RefreshTokenAsync(tokenRequest);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest("Invalid details");

        }
    }
}
