using backend_app.Domain.Models;
using System.Threading.Tasks;

namespace backend_app.Domain.Services
{
    public interface ISignUpService
    {
        Task<Response<Account>> RegisterAccount(Account account);
    }
}