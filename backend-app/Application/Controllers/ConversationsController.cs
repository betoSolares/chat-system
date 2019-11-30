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
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public ConversationsController(IAccountService accountService, IConversationService conversationService, IMapper mapper)
        {
            _accountService = accountService;
            _conversationService = conversationService;
            _mapper = mapper;
        }
        // Get all of the conversations
        [ActionName("conversations")]
        [Route("api/[controller]/[action]/{username}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetConversations(string username)
        {
            if (ModelState.IsValid)
            {
                Response<List<Conversation>> response = await _conversationService.GetConversations(username);
                if (response.Succes)
                {
                    List<ConversationResource> conversations = new List<ConversationResource>();
                    foreach (Conversation conversation in response.Resource)
                    {
                        string otherMember = conversation.Members.Find(m => !m.Equals(username));
                        Response<Account> newResponse = await _accountService.GetInformation(otherMember);
                        ConversationResource resource = _mapper.Map<Account, ConversationResource>(newResponse.Resource);
                        conversations.Add(resource);
                    }
                    return Ok(new { conversations });
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
        // Get all of the conversations
        [ActionName("messages")]
        [Route("api/[controller]/[action]/{username}/{contact}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMessages(string username, string contact)
        {
            if (ModelState.IsValid)
            {
                Response<List<Message>> response = await _conversationService.GetMessages(username, contact);
                if (response.Succes)
                {
                    List<MessageResource> messages = new List<MessageResource>();
                    foreach (Message message in response.Resource)
                    {
                        MessageResource resource = _mapper.Map<Message, MessageResource>(message);
                        messages.Add(resource);
                    }
                    return Ok(new { messages });
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
        // Send a new message
        [ActionName("send")]
        [Route("api/[controller]/[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendMessage(MessageReceiveResource messageReceiveResource)
        {
            if (ModelState.IsValid)
            {
                Response<Conversation> response = await _conversationService.SendMessage(messageReceiveResource.From, messageReceiveResource.To, messageReceiveResource.Content);
                if (response.Succes)
                {
                    return Ok(new { Conversation = response.Resource });
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
        // Remove an existing message
        [ActionName("delete")]
        [Route("api/[controller]/[action]")]
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveMessage(MessageReceiveResource messageReceiveResource)
        {
            if (ModelState.IsValid)
            {
                Response<Conversation> response = await _conversationService.RemoveMessage(messageReceiveResource.From, messageReceiveResource.To, messageReceiveResource.Content);
                if (response.Succes)
                {
                    return Ok(new { Conversation = response.Resource });
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
        // Remove an existing message
        [ActionName("modify")]
        [Route("api/[controller]/[action]")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ModifyMessage(MessageReceiveResource messageReceiveResource)
        {
            if (ModelState.IsValid)
            {
                Response<Conversation> response = await _conversationService.ModifiedMessage(messageReceiveResource.From, messageReceiveResource.To, messageReceiveResource.Content, messageReceiveResource.NewContent);
                if (response.Succes)
                {
                    return Ok(new { Conversation = response.Resource });
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