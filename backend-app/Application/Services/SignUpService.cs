using System;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;

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

        public async Task<Response<Account>> RegisterAccount(Account account)
        {
            try
            {
                if (_accountCollection.Get(account.Id) == null)
                {
                    Account newAccount = await _accountCollection.Create(account);
                    return new Response<Account>(newAccount);
                }
                else
                {
                    return new Response<Account>("The account alredy exists", 409);
                }
            }
            catch (Exception ex)
            {
                return new Response<Account>(ex.ToString(), 500);
            }
        }
    }
}