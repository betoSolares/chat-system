using backend_app.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace backend_app.Domain.Services
{
    public interface IConversationService
    {
        Task<Response<Conversation>> SendMessage(string username, string contact, string content);
        Task<Response<Conversation>> RemoveMessage(string username, string contact, string content);
        Task<Response<Conversation>> ModifiedMessage(string username, string contact, string content, string newContent);
        Task<Response<List<Message>>> GetMessages(string username, string otheruser);
        Task<Response<List<Conversation>>> GetConversations(string username);
    }
}