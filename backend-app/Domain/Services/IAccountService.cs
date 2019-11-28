using backend_app.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_app.Domain.Services
{
    public interface IAccountService
    {
        Task<Response<List<Account>>> GetSpecific(string username);
        Task<Response<Account>> GetInformation(string username);
    }
}