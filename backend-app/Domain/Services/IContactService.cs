using backend_app.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_app.Domain.Services
{
    public interface IContactService
    {
        Task<Response<Contact>> AcceptRequest(string username, string otheruser);
        Task<Response<Contact>> NegateRequest(string username, string otheruser);
        Task<Response<Contact>> DeleteRequest(string username, string otheruser);
        Task<Response<Contact>> SentRequest(string username, string otheruser);
        Task<Response<List<string>>> GetIncoming(string username);
        Task<Response<List<string>>> GetContacts(string username);
        Task<Response<List<string>>> GetSent(string username);
    }
}