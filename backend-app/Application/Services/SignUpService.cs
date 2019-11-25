using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
using external_process.Encryption;

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

        /// <summary>Try to create a new account</summary>
        /// <param name="account">The account to add</param>
        /// <returns>A succesful response if it's created, otherwise failed response</returns>
        public async Task<Response<Account>> RegisterAccount(Account account)
        {
            try
            {
                if (_accountCollection.GetByUsername(account.Username) == null)
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

        /// <summary>Add salt and encrypt the password</summary>
        /// <param name="account">The account with the password</param>
        /// <returns>A new account with a password encrypted</returns>
        private Account EncryptPassword(Account account)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                account.Salt = hmac.Key;
            }
            byte[] password = Encoding.UTF8.GetBytes(account.Password + account.Salt);
            int key = int.Parse(Environment.GetEnvironmentVariable("SDES_KEY"));
            List<byte> hashedPassword = new List<byte>();
            foreach (byte _byte in password)
            {
                hashedPassword.Add(new SDES().Encrypt(_byte, key));
            }
            account.Password = Encoding.UTF8.GetString(hashedPassword.ToArray());
            return account;
        }
    }
}