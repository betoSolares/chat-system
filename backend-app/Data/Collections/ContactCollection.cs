using System.Collections.Generic;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Contexts;
using backend_app.Domain.Models;
using MongoDB.Driver;

namespace backend_app.Data.Collections
{
    public class ContactCollection : IContactCollection
    {
        private readonly IMongoCollection<Contact> _contact;
        
        /// <summary>Contructor</summary>
        /// <param name="context">The context of the collection</param>
        public ContactCollection(IDatabaseContext context)
        {
            MongoClient client = new MongoClient(context.ConnectionString);
            IMongoDatabase database = client.GetDatabase(context.DatabaseName);
            _contact = database.GetCollection<Contact>("Contacts");
        }
        
        /// <summary>Add a new contact</summary>
        /// <param name="username">The user that adds the contact</param>
        /// <param name="contactname">The name of the contact to add</param>
        /// <returns>The new contact</returns>
        public async Task<Contact> AddContact(string username, string contactname)
        {
            Contact contact = await GetAll(username);
            contact.Sent.Remove(contactname);
            contact.Incoming.Remove(contactname);
            contact.Contacts.Add(contactname);
            await _contact.ReplaceOneAsync(c => c.Username.Equals(contact.Username), contact);
            return contact;
        }
        
        /// <summary>Add a new contact</summary>
        /// <param name="username">The user that adds the contact</param>
        /// <param name="contactname">The name of the contact to add</param>
        /// <returns>The new contact</returns>
        public async Task<Contact> AddIncoming(string username, string contactname)
        {
            Contact contact = await GetAll(username);
            if (contact == null)
            {
                contact = new Contact
                {
                    Username = username,
                    Incoming = new List<string>() { contactname },
                    Sent = new List<string>(),
                    Contacts = new List<string>()
                };
                await _contact.InsertOneAsync(contact);
            }
            else
            {
                contact.Incoming.Add(contactname);
                await _contact.ReplaceOneAsync(c => c.Id.Equals(contact.Id), contact);
            }
            return contact;
        }
        
        /// <summary>Add a new contact</summary>
        /// <param name="username">The user that adds the contact</param>
        /// <param name="contactname">The name of the contact to add</param>
        /// <returns>The new contact</returns>
        public async Task<Contact> AddSent(string username, string contactname)
        {
            Contact contact = await GetAll(username);
            if (contact == null)
            {
                contact = new Contact
                {
                    Username = username,
                    Sent = new List<string>() { contactname },
                    Incoming = new List<string>(),
                    Contacts = new List<string>()
                };
                await _contact.InsertOneAsync(contact);
            }
            else
            {
                contact.Sent.Add(contactname);
                await _contact.ReplaceOneAsync(c => c.Id.Equals(contact.Id), contact);
            }
            return contact;
        }
        
        /// <summary>Delete all the elements for one item</summary>
        /// <param name="username">The user to delete</param>
        public async Task DeleteAll(string username) => await _contact.DeleteOneAsync(c => c.Username.Equals(username));
        
        /// <summary>Delete a contact</summary>
        /// <param name="username">The user that deletes the contact</param>
        /// <param name="contactname">The name of the other user</param>
        /// <returns>The new contact</returns>
        public async Task<Contact> DeleteContact(string username, string contactname)
        {
            Contact contact = await GetAll(username);
            contact.Contacts.Remove(contactname);
            await _contact.ReplaceOneAsync(c => c.Id.Equals(contact.Id), contact);
            return contact;
        }
        
        /// <summary>Delete a incoming request</summary>
        /// <param name="username">The user that deletes the request</param>
        /// <param name="contactname">The name of the other user</param>
        /// <returns>The new contact</returns>
        public async Task<Contact> DeleteIncoming(string username, string contactname)
        {
            Contact contact = await GetAll(username);
            contact.Incoming.Remove(contactname);
            await _contact.ReplaceOneAsync(c => c.Id.Equals(contact.Id), contact);
            return contact;
        }
        
        /// <summary>Delete a sent request</summary>
        /// <param name="username">The user that deletes the request</param>
        /// <param name="contactname">The name of the other user</param>
        /// <returns>The new contact</returns>
        public async Task<Contact> DeleteSent(string username, string contactname)
        {
            Contact contact = await GetAll(username);
            contact.Sent.Remove(contactname);
            await _contact.ReplaceOneAsync(c => c.Id.Equals(contact.Id), contact);
            return contact;
        }
        
        /// <summary>Get all the items for  one user</summary>
        /// <returns>A list with all the items in the collection</returns>
        public async Task<Contact> GetAll(string usernme)
        {
            IAsyncCursor<Contact> task = await _contact.FindAsync(c => c.Username.Equals(usernme));
            Contact contact = await task.FirstOrDefaultAsync();
            return contact;
        }
        
        /// <summary>Get all the contacts for one account</summary>
        /// <param name="username">The username of the account</param>
        /// <returns>A list with all the contacts</returns>
        public async Task<List<string>> GetContacts(string username)
        {
            IAsyncCursor<Contact> task = await _contact.FindAsync(c => c.Username.Equals(username));
            Contact contact = await task.FirstOrDefaultAsync();
            if (contact == null)
                return new List<string>();
            else
                return contact.Contacts;
        }
        
        /// <summary>Get all the incoming requests for one account</summary>
        /// <param name="username">The username of the account</param>
        /// <returns>A list with all the incoming request</returns>
        public async Task<List<string>> GetIncoming(string username)
        {
            IAsyncCursor<Contact> task = await _contact.FindAsync(c => c.Username.Equals(username));
            Contact contact = await task.FirstOrDefaultAsync();
            return contact.Incoming;
        }
        
        /// <summary>Get all the sent request for one account</summary>
        /// <param name="username">The username of the account</param>
        /// <returns>A list with all the sent request</returns>
        public async Task<List<string>> GetSent(string username)
        {
            IAsyncCursor<Contact> task = await _contact.FindAsync(c => c.Username.Equals(username));
            Contact contact = await task.FirstOrDefaultAsync();
            return contact.Sent;
        }
    }
}