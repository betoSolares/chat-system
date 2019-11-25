using AutoMapper;
using backend_app.Application.Resources;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend_app.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISignUpService _signupService;

        public AccountController(IMapper mapper, ISignUpService signUpService)
        {
            _mapper = mapper;
            _signupService = signUpService;
        }

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
                    return Ok("Success");
                }
                else
                {
                    if (response.StatusCode == 409)
                        return Conflict(response.Message);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}