using AutoMapper;
using backend_app.Application.Resources;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace backend_app.Application.Controllers
{
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IAccountService accountService, IContactService contactService, IMapper mapper)
        {
            _accountService = accountService;
            _contactService = contactService;
            _mapper = mapper;
        }

        // Get all of the contacts
        [ActionName("mycontacts")]
        [Route("api/[controller]/[action]/{username}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetContacts(string username)
        {
            if (ModelState.IsValid)
            {
                Response<List<string>> response = await _contactService.GetContacts(username);
                if (response.Succes)
                {
                    List<ContactResource> contacts = new List<ContactResource>();
                    foreach (string contact in response.Resource)
                    {
                        Response<Account> newResponse = await _accountService.GetInformation(contact);
                        ContactResource resource = _mapper.Map<Account, ContactResource>(newResponse.Resource);
                        contacts.Add(resource);
                    }
                    return Ok(new { contacts });
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

        // Get specific contacts
        [ActionName("specific")]
        [Route("api/[controller]/[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetSpecificContacts(FindContactResource findContactResource)
        {
            if (ModelState.IsValid)
            {
                Response<List<Account>> response = await _accountService.GetSpecific(findContactResource.Otheruser);
                if (response.Succes)
                {
                    Response<List<string>> newResponse = await _contactService.GetSent(findContactResource.Username);
                    List<string> sentRequests = newResponse.Resource;
                    List<ContactResource> contacts = new List<ContactResource>();
                    foreach (Account contact in response.Resource)
                    {
                        ContactResource resource = _mapper.Map<Account, ContactResource>(contact);
                        if (sentRequests != null && sentRequests.Contains(contact.Username))
                            resource.SentRequest = true;
                        contacts.Add(resource);
                    }
                    newResponse = await _contactService.GetContacts(findContactResource.Username);
                    if (newResponse.Resource != null && newResponse.Resource.Count != 0)
                    {
                        foreach (string myContact in newResponse.Resource)
                        {
                            contacts.RemoveAll(x => x.Username.Equals(myContact));
                        }
                    }
                    return Ok(new { contacts });
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

        // Add a new sent request
        [ActionName("add")]
        [Route("api/[controller]/[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> MakeRequest(FindContactResource findContactResource)
        {
            if (ModelState.IsValid)
            {
                Response<Contact> response = await _contactService.SentRequest(findContactResource.Username, findContactResource.Otheruser);
                if (response.Succes)
                {

                    return Ok(new { contact = response.Resource });
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

        // Remove a request that was sent
        [ActionName("remove")]
        [Route("api/[controller]/[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveRequest(FindContactResource findContactResource)
        {
            if (ModelState.IsValid)
            {
                Response<Contact> response = await _contactService.DeleteRequest(findContactResource.Username, findContactResource.Otheruser);
                if (response.Succes)
                {

                    return Ok(new { contact = response.Resource });
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

        // Get all of my incomig requests
        [ActionName("requests")]
        [Route("api/[controller]/[action]/{username}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRequests(string username)
        {
            if (ModelState.IsValid)
            {
                Response<List<string>> response = await _contactService.GetIncoming(username);
                if (response.Succes)
                {
                    List<ContactResource> contacts = new List<ContactResource>();
                    foreach (string contact in response.Resource)
                    {
                        Response<Account> newResponse = await _accountService.GetInformation(contact);
                        ContactResource resource = _mapper.Map<Account, ContactResource>(newResponse.Resource);
                        contacts.Add(resource);
                    }
                    return Ok(new { contacts });
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

        // Accept an incoming request
        [ActionName("accept")]
        [Route("api/[controller]/[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AcceptRequest(FindContactResource findContactResource)
        {
            if (ModelState.IsValid)
            {
                Response<Contact> response = await _contactService.AcceptRequest(findContactResource.Username, findContactResource.Otheruser);
                if (response.Succes)
                {

                    return Ok(new { contact = response.Resource });
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

        // Decline a incoming request
        [ActionName("decline")]
        [Route("api/[controller]/[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeclineRequest(FindContactResource findContactResource)
        {
            if (ModelState.IsValid)
            {
                Response<Contact> response = await _contactService.NegateRequest(findContactResource.Username, findContactResource.Otheruser);
                if (response.Succes)
                {
                    return Ok(new { contact = response.Resource });
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
    }
}