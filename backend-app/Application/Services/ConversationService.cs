using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Models;
using backend_app.Domain.Services;
namespace backend_app.Application.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationCollection _conversationCollection;
        /// <summary>Constructor</summary>
        /// <param name="conversationCollection">The conversation collection</param>
        public ConversationService(IConversationCollection conversationCollection)
        {
            _conversationCollection = conversationCollection;
        }
        /// <summary>Get all the conversations from an specific user</summary>
        /// <param name="username">The user to get the conversations</param>
        /// <returns>A list with all the conversations</returns>
        public async Task<Response<List<Conversation>>> GetConversations(string username)
        {
            try
            {
                List<Conversation> conversations = await _conversationCollection.GetConversations(username);
                if (conversations != null && conversations.Count != 0)
                {
                    return new Response<List<Conversation>>(conversations);
                }
                else
                {
                    return new Response<List<Conversation>>("No conversations", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Conversation>>(ex.ToString(), 500);
            }
        }
        /// <summary>Get all the messages from an specific conversation</summary>
        /// <param name="username">The user that makes the request</param>
        /// <param name="otheruser">The other member</param>
        /// <returns>A list with all the messages</returns>
        public async Task<Response<List<Message>>> GetMessages(string username, string otheruser)
        {
            try
            {
                Conversation conversation = await _conversationCollection.GetMessages(username, otheruser);
                if (conversation != null && conversation.Messages.Count != 0)
                {
                    return new Response<List<Message>>(conversation.Messages);
                }
                else
                {
                    return new Response<List<Message>>("No messages", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Message>>(ex.ToString(), 500);
            }
        }
        /// <summary>Modified an existing message</summary>
        /// <param name="username">The user that makes the request</param>
        /// <param name="contact">The other member of the conversation</param>
        /// <param name="content">The old content</param>
        /// <param name="newContent">The new message</param>
        /// <returns>The new conversation</returns>
        public async Task<Response<Conversation>> ModifiedMessage(string username, string contact, string content, string newContent)
        {
            try
            {
                Conversation conversation = await _conversationCollection.ModifiedMessage(username, contact, content, newContent);
                if (conversation != null)
                {
                    return new Response<Conversation>(conversation);
                }
                else
                {
                    return new Response<Conversation>("Not messages modified", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Conversation>(ex.ToString(), 500);
            }
        }
        /// <summary>Remove a message from a conversation</summary>
        /// <param name="username">The user that make the request</param>
        /// <param name="contact">The ohter member</param>
        /// <param name="content">The content of the message to remove</param>
        /// <returns>The new conversation</returns>
        public async Task<Response<Conversation>> RemoveMessage(string username, string contact, string content)
        {
            try
            {
                Conversation conversation = await _conversationCollection.DeleteMessage(username, contact, content);
                if (conversation != null)
                {
                    return new Response<Conversation>(conversation);
                }
                else
                {
                    return new Response<Conversation>("Not messages", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Conversation>(ex.ToString(), 500);
            }
        }
        /// <summary>Add a new message to the conversation</summary>
        /// <param name="username">The user that make the request</param>
        /// <param name="contact">The other member of the conversation</param>
        /// <param name="content">The new message</param>
        /// <returns>The new conversation</returns>
        public async Task<Response<Conversation>> SendMessage(string username, string contact, string content)
        {
            try
            {
                Conversation conversation = await _conversationCollection.AddMessage(username, contact, content);
                if (conversation != null)
                {
                    return new Response<Conversation>(conversation);
                }
                else
                {
                    return new Response<Conversation>("Not messages added", 404);
                }
            }
            catch (Exception ex)
            {
                return new Response<Conversation>(ex.ToString(), 500);
            }
        }
    }
}