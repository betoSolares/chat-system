using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;

namespace backend_app.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountCollection _accountCollection;
        
        /// <summary>Constructor</summary>
        /// <param name="accountCollection">The collection of accounts</param>
        public AccountService(IAccountCollection accountCollection)
        {
            _accountCollection = accountCollection;
        }
        
        /// <summary>Get all the information of the accounts</summary>
        /// <returns>A list with all the information</returns>
        public async Task<Response<List<Account>>> GetSpecific(string username)
        {
            try
            {
                List<Account> account = await _accountCollection.GetAll();
                if (account != null && account.Count != 0)
                {
                    List<Account> specifics = new List<Account>();
                    foreach (Account _account in account)
                    {
                        if (_account.Username.Contains(username))
                        {
                            specifics.Add(_account);
                        }
                    }
                    return new Response<List<Account>>(specifics);
                }

                else
                    return new Response<List<Account>>("No account", 404);
            }
            catch (Exception ex)
            {
                return new Response<List<Account>>(ex.ToString(), 500);
            }
        }
        
        /// <summary>Get all the info of one account</summary>
        /// <param name="username">The user of the account</param>
        /// <returns>The information of the account</returns>
        public async Task<Response<Account>> GetInformation(string username)
        {
            try
            {
                Account account = await _accountCollection.GetByUsername(username);
                if (account != null)
                    return new Response<Account>(account);
                else
                    return new Response<Account>("No account", 404);
            }
            catch (Exception ex)
            {
                return new Response<Account>(ex.ToString(), 500);
            }
        }
    }
}