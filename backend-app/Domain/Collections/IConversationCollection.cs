using backend_app.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace backend_app.Domain.Collections
{
    public interface IConversationCollection
    {
        Task<List<Conversation>> GetConversations(string username);
        Task<Conversation> GetMessages(string username, string contact);
        Task<Conversation> AddMessage(string username, string contact, string content);
        Task<Conversation> DeleteMessage(string username, string contact, string content);
        Task<Conversation> ModifiedMessage(string username, string contact, string content, string newContent);
        Task<Conversation> AddFile(string username, string contact, string content, string path);
        Task DeleteConversation(string id);
    }
}