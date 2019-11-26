using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
using external_process.Encryption;
using Microsoft.Extensions.Configuration;
​
namespace backend_app.Application.Services
{
    public class LogInService : ILogInService
    {
        private readonly IAccountCollection _accountCollection;
        private readonly IConfiguration _configuration;
​
        /// <summary>Constructor</summary>
        /// <param name="accountCollection">The account collection</param>
        /// <param name="configuration">The configuration of the application</param>
        public LogInService(IAccountCollection accountCollection, IConfiguration configuration)
        {
            _accountCollection = accountCollection;
            _configuration = configuration;
        }
​
        /// <summary>Try to access de data of the account</summary>
        /// <param name="account">The account to access</param>
        /// <returns>A succesful response the account data is correct, otherwise failed response</returns>
        public async Task<Response<Account>> LogIn(Account account)
        {
            try
            {
                Account newAccount = await _accountCollection.GetByUsername(account.Username);
                if (newAccount != null)
                {
                    string password = EncryptPassword(newAccount.Salt, account.Password);
                    if (password.Equals(newAccount.Password))
                        return new Response<Account>(newAccount);
                    else
                        return new Response<Account>("Bad username or password", 404);
                }
                else
                {
                    return new Response<Account>("Bad username or password", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Account>(ex.ToString(), 500);
            }
        }
​
        /// <summary>Add salt and encrypt the password</summary>
        /// <param name="password">The account with the password</param>
        /// <returns>A new account with a password encrypted</returns>
        private string EncryptPassword(byte[] salt, string password)
        {
            byte[] _password = Encoding.UTF8.GetBytes(password + salt);
            int key = int.Parse(_configuration["SDES_KEY"]);
            List<byte> hashedPassword = new List<byte>();
            foreach (byte _byte in _password)
            {
                hashedPassword.Add(new SDES().Encrypt(_byte, key));
            }
            return Encoding.UTF8.GetString(hashedPassword.ToArray());
        }
    }
}