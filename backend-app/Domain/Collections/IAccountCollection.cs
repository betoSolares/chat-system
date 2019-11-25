using backend_app.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_app.Domain.Collections
{
    public interface IAccountCollection
    {
        Task<List<Account>> GetAll();
        Task<Account> GetById(string id);
        Task<Account> GetByUsername(string username);
        Task<Account> Create(Account account);
        Task<Account> Update(string id, Account account);
        Task Delete(string id);
    }
}
