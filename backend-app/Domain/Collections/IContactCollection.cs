using backend_app.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_app.Domain.Collections
{
    public interface IContactCollection
    {
        Task<Contact> GetAll(string username);
        Task<List<string>> GetContacts(string username);
        Task<List<string>> GetSent(string username);
        Task<List<string>> GetIncoming(string username);
        Task<Contact> AddContact(string username, string contact);
        Task<Contact> AddSent(string username, string contact);
        Task<Contact> AddIncoming(string username, string contact);
        Task<Contact> DeleteContact(string username, string contact);
        Task<Contact> DeleteSent(string username, string contact);
        Task<Contact> DeleteIncoming(string username, string contact);
        Task DeleteAll(string username);
    }
}