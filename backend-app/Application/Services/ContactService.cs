using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
​
namespace backend_app.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IAccountCollection _accountCollection;
        private readonly IContactCollection _contactCollection;

        /// <summary>Constructor</summary>
        /// <param name="accountCollection">The account collection</param>
        /// <param name="configuration">The configuration of the application</param>
        public ContactService(IAccountCollection accountCollection, IContactCollection contactCollection)
        {
            _accountCollection = accountCollection;
            _contactCollection = contactCollection;
        }

        /// <summary>Accept a new request</summary>
        /// <param name="username">The username</param>
        /// <param name="otheruser">The username of the other account</param>
        /// <returns>A succesful response the account data is correct, otherwise failed response</returns>
        public async Task<Response<Contact>> AcceptRequest(string username, string otheruser)
        {
            try
            {
                if (await _accountCollection.GetByUsername(otheruser) != null)
                {
                    await _contactCollection.AddContact(otheruser, username);
                    Contact contact = await _contactCollection.AddContact(username, otheruser);
                    return new Response<Contact>(contact);
                }
                else
                {
                    return new Response<Contact>("User not found", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Contact>(ex.ToString(), 500);
            }
        }

        /// <summary>Delete a request that was sent</summary>
        /// <param name="username">The username</param>
        /// <param name="otheruser">The username of the other account</param>
        /// <returns>A succesful response the account data is correct, otherwise failed response</returns>
        public async Task<Response<Contact>> DeleteRequest(string username, string otheruser)
        {
            try
            {
                if (await _accountCollection.GetByUsername(otheruser) != null)
                {
                    await _contactCollection.DeleteIncoming(otheruser, username);
                    Contact contact = await _contactCollection.DeleteSent(username, otheruser);
                    return new Response<Contact>(contact);
                }
                else
                {
                    return new Response<Contact>("User not found", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Contact>(ex.ToString(), 500);
            }
        }

        /// <summary>Get all the contacts</summary>
        /// <param name="username">The user to get the contacts</param>
        /// <returns>A list with all the contacts</returns>
        public async Task<Response<List<string>>> GetContacts(string username)
        {
            try
            {
                List<string> contacts = await _contactCollection.GetContacts(username);
                if (contacts.Count != 0)
                {
                    return new Response<List<string>>(contacts);
                }
                else
                {
                    return new Response<List<string>>("No contacts", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<string>>(ex.ToString(), 500);
            }
        }

        /// <summary>Get all the incoming request</summary>
        /// <param name="username">The user to get the requests</param>
        /// <returns>A list with all the incoming request</returns>
        public async Task<Response<List<string>>> GetIncoming(string username)
        {
            try
            {
                List<string> contacts = await _contactCollection.GetIncoming(username);
                if (contacts != null && contacts.Count != 0)
                {
                    return new Response<List<string>>(contacts);
                }
                else
                {
                    return new Response<List<string>>("No incoming requests", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<string>>(ex.ToString(), 500);
            }
        }

        /// <summary>Get all the sent request</summary>
        /// <param name="username">The user to get the requests</param>
        /// <returns>A list with all the sent request</returns>
        public async Task<Response<List<string>>> GetSent(string username)
        {
            try
            {
                List<string> contacts = await _contactCollection.GetSent(username);
                if (contacts != null && contacts.Count != 0)
                {
                    return new Response<List<string>>(contacts);
                }
                else
                {
                    return new Response<List<string>>("No sent requests", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<string>>(ex.ToString(), 500);
            }
        }

        /// <summary>Delete an incoming request</summary>
        /// <param name="username">The username</param>
        /// <param name="otheruser">The username of the other account</param>
        /// <returns>A succesful response the account data is correct, otherwise failed response</returns>
        public async Task<Response<Contact>> NegateRequest(string username, string otheruser)
        {
            try
            {
                if (await _accountCollection.GetByUsername(otheruser) != null)
                {
                    await _contactCollection.DeleteSent(otheruser, username);
                    Contact contact = await _contactCollection.DeleteIncoming(username, otheruser);
                    return new Response<Contact>(contact);
                }
                else
                {
                    return new Response<Contact>("User not found", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Contact>(ex.ToString(), 500);
            }
        }

        /// <summary>Sent a new request</summary>
        /// <param name="username">The username</param>
        /// <param name="otheruser">The username of the other account</param>
        /// <returns>A succesful response the account data is correct, otherwise failed response</returns>
        public async Task<Response<Contact>> SentRequest(string username, string otheruser)
        {
            try
            {
                if (await _accountCollection.GetByUsername(otheruser) != null)
                {
                    await _contactCollection.AddIncoming(otheruser, username);
                    Contact contact = await _contactCollection.AddSent(username, otheruser);
                    return new Response<Contact>(contact);
                }
                else
                {
                    return new Response<Contact>("User not found", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Contact>(ex.ToString(), 500);
            }
        }
    }
}