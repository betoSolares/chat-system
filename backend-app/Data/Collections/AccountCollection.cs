using backend_app.Domain.Contexts;
using backend_app.Domain.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace backend_app.Data.Collections
{
    public class AccountCollection
    {
        private readonly IMongoCollection<Account> _account;

        /// <summary>Contructor</summary>
        /// <param name="context">The context of the collection</param>
        public AccountCollection(IAccountContext context)
        {
            MongoClient client = new MongoClient(context.ConnectionString);
            IMongoDatabase database = client.GetDatabase(context.DatabaseName);
            _account = database.GetCollection<Account>(context.CollectionName);
        }

        /// <summary>Get all items in the account collection</summary>
        /// <returns>A list with each collection</returns>
        public List<Account> Get() => _account.Find(a => true).ToList();

        /// <summary>Get all the data for an specific account</summary>
        /// <param name="id">The id of the account</param>
        /// <returns>The data for the specific account</returns>
        public Account Get(string id) => _account.Find(a => a.Id.Equals(id)).FirstOrDefault();

        /// <summary>Add a new account in the collection</summary>
        /// <param name="account">The account to add</param>
        /// <returns>The account that was added</returns>
        public Account Create(Account account)
        {
            _account.InsertOne(account);
            return account;
        }

        /// <summary>Update one account in the collection</summary>
        /// <param name="id">The id of the account</param>
        /// <param name="account">The new information for the account</param>
        /// <returns>The new data for the account</returns>
        public Account Update(string id, Account account)
        {
            _account.ReplaceOne(a => a.Id.Equals(id), account);
            return account;
        }

        /// <summary>Remove one account from the collection</summary>
        /// <param name="id">The id of the account to delete</param>
        public void Delete(string id) => _account.DeleteOne(a => a.Id.Equals(id));
    }
}