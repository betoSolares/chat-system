using System;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
using backend_app.Domain.Services.Communication;

namespace backend_app.Application.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly IAccountCollection _accountCollection;

        /// <summary>Constructor</summary>
        /// <param name="accountCollection">The account collection</param>
        public SignUpService(IAccountCollection accountCollection)
        {
            _accountCollection = accountCollection;
        }

        public async Task<SignUpResponse> RegisterAccount(Account account)
        {
            try
            {
                if (_accountCollection.Get(account.Id) == null)
                {
                    Account newAccount = await _accountCollection.Create(account);
                    return new SignUpResponse(newAccount);
                }
                else
                {
                    return new SignUpResponse("Conflict");
                }
            }
            catch (Exception ex)
            {
                return new SignUpResponse(ex.ToString());
            }
        }
    }
}