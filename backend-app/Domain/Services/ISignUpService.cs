using backend_app.Domain.Models;
using backend_app.Domain.Services.Communication;
using System.Threading.Tasks;

namespace backend_app.Domain.Services
{
    public interface ISignUpService
    {
        Task<SignUpResponse> RegisterAccount(Account account);
    }
}