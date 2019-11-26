using backend_app.Domain.Collections;
using backend_app.Domain.Contexts;
using backend_app.Domain.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_app.Data.Collections
{
    public class AccountCollection : IAccountCollection
    {
        private readonly IMongoCollection<Account> _account;

        /// <summary>Contructor</summary>
        /// <param name="context">The context of the collection</param>
        public AccountCollection(IDatabaseContext context)
        {
            MongoClient client = new MongoClient(context.ConnectionString);
            IMongoDatabase database = client.GetDatabase(context.DatabaseName);
            _account = database.GetCollection<Account>("Accounts");
        }

        /// <summary>Get all items in the account collection</summary>
        /// <returns>A list with each collection</returns>
        public async Task<List<Account>> GetAll()
        {
            IAsyncCursor<Account> task = await _account.FindAsync(a => true);
            List<Account> list = await task.ToListAsync();
            return list;
        }

        /// <summary>Get all the data for an specific account</summary>
        /// <param name="id">The id of the account</param>
        /// <returns>The data for the specific account</returns>
        public async Task<Account> GetById(string id)
        {
            IAsyncCursor<Account> task = await _account.FindAsync(a => a.Id.Equals(id));
            List<Account> list = await task.ToListAsync();
            Account result = list.FirstOrDefault();
            return result;
        }

        /// <summary>Get all the data for an specific account</summary>
        /// <param name="username">The username of the account</param>
        /// <returns>The data for the specific account</returns>
        public async Task<Account> GetByUsername(string username)
        {
            IAsyncCursor<Account> task = await _account.FindAsync(a => a.Username.Equals(username));
            List<Account> list = await task.ToListAsync();
            Account result = list.FirstOrDefault();
            return result;
        }

        /// <summary>Add a new account in the collection</summary>
        /// <param name="account">The account to add</param>
        /// <returns>The account that was added</returns>
        public async Task<Account> Create(Account account)
        {
            await _account.InsertOneAsync(account);
            return account;
        }

        /// <summary>Update one account in the collection</summary>
        /// <param name="id">The id of the account</param>
        /// <param name="account">The new information for the account</param>
        /// <returns>The new data for the account</returns>
        public async Task<Account> Update(string id, Account account)
        {
            await _account.ReplaceOneAsync(a => a.Id.Equals(id), account);
            return account;
        }

        /// <summary>Remove one account from the collection</summary>
        /// <param name="id">The id of the account to delete</param>
        public async Task Delete(string id) => await _account.DeleteOneAsync(a => a.Id.Equals(id));
    }
}