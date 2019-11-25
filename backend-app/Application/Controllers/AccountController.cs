using AutoMapper;
using backend_app.Application.Resources;
using backend_app.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend_app.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [ActionName("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpResource signUpResource)
        {
            if (ModelState.IsValid)
            {
                Account account = _mapper.Map<SignUpResource, Account>(signUpResource);
            }
            return BadRequest(ModelState);
        }
    }
}