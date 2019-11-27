using AutoMapper;
using backend_app.Application.Resources;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend_app.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogInService _logInService;
        private readonly IMapper _mapper;
        private readonly ISignUpService _signupService;
        
        public AccountController(IConfiguration configuration, ILogInService logInService, IMapper mapper,
                                 ISignUpService signUpService)
        {
            _configuration = configuration;
            _logInService = logInService;
            _mapper = mapper;
            _signupService = signUpService;
        }
        
        // Create a new account
        [ActionName("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpResource signUpResource)
        {
            if (ModelState.IsValid)
            {
                Account account = _mapper.Map<SignUpResource, Account>(signUpResource);
                Response<Account> response = await _signupService.RegisterAccount(account);
                if (response.Succes)
                {
                    string token = BuildToken(response.Resource);
                    return Ok(new { token });
                }
                else
                {
                    if (response.StatusCode == 409)
                        return Conflict(new { error = response.Message });
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = response.Message });
                }
            }
            return BadRequest(ModelState);
        }
        
        // Access to the data of your account
        [ActionName("login")]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInResource logInResource)
        {
            if (ModelState.IsValid)
            {
                Account account = _mapper.Map<LogInResource, Account>(logInResource);
                Response<Account> response = await _logInService.LogIn(account);
                if (response.Succes)
                {
                    string token = BuildToken(response.Resource);
                    return Ok(new { token });
                }
                else
                {
                    if (response.StatusCode == 404)
                        return NotFound(new { error = response.Message });
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = response.Message });
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>Create a new token</summary>
        /// <param name="account">The data for the token</param>
        /// <returns>The new token</returns>
        private string BuildToken(Account account)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.GivenName, account.GivenName),
                new Claim(JwtRegisteredClaimNames.FamilyName, account.FamilyName),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "chat-system/backend-app",
                audience: "chat-system/frontend-app",
                claims: claims,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}