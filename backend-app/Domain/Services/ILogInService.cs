using backend_app.Domain.Models;
using System.Threading.Tasks;

namespace backend_app.Domain.Services
{
    public interface ILogInService
    {
        Task<Response<Account>> LogIn(Account account);
    }
}