using System.Collections.Generic;
using System.Threading.Tasks;
using backend_app.Domain.Collections;
using backend_app.Domain.Contexts;
using backend_app.Domain.Models;
using MongoDB.Driver;
namespace backend_app.Data.Collections
{
    public class ConversationCollection : IConversationCollection
    {
        private readonly IMongoCollection<Conversation> _conversation;
        /// <summary>Contructor</summary>
        /// <param name="context">The context of the collection</param>
        public ConversationCollection(IDatabaseContext context)
        {
            MongoClient client = new MongoClient(context.ConnectionString);
            IMongoDatabase database = client.GetDatabase(context.DatabaseName);
            _conversation = database.GetCollection<Conversation>("Conversations");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="contact"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<Conversation> AddFile(string username, string contact, string content, string path)
        {
            Conversation conversation = await GetMessages(username, contact);
            Message message = new Message()
            {
                From = username,
                IsFile = true,
                Content = content,
                Path = path
            };
            if (conversation == null)
            {
                conversation = new Conversation()
                {
                    Members = new List<string>() { username, contact },
                    Messages = new List<Message>() { message }
                };
                await _conversation.InsertOneAsync(conversation);
                return conversation;
            }
            else
            {
                conversation.Messages.Add(message);
                await _conversation.ReplaceOneAsync(c => c.Id.Equals(conversation.Id), conversation);
                return conversation;
            }
        }
        /// <summary>Add a new message to the conversation</summary>
        /// <param name="username">The username that is making the request</param>
        /// <param name="contact">The other member of the conversation</param>
        /// <param name="content">The content of the new message</param>
        /// <returns>The new conversation</returns>
        public async Task<Conversation> AddMessage(string username, string contact, string content)
        {
            Conversation conversation = await GetMessages(username, contact);
            Message message = new Message()
            {
                From = username,
                IsFile = false,
                Content = content,
                Path = null
            };
            if (conversation == null)
            {
                conversation = new Conversation()
                {
                    Members = new List<string>() { username, contact },
                    Messages = new List<Message>() { message }
                };
                await _conversation.InsertOneAsync(conversation);
                return conversation;
            }
            else
            {
                conversation.Messages.Add(message);
                await _conversation.ReplaceOneAsync(c => c.Id.Equals(conversation.Id), conversation);
                return conversation;
            }
        }
        /// <summary>Delete an entire conversation</summary>
        /// <param name="id">The id of the conversation</param>
        public async Task DeleteConversation(string id) => await _conversation.DeleteOneAsync(c => c.Id.Equals(id));
        /// <summary>Delete a message from a conversation</summary>
        /// <param name="username">The user that makes the request</param>
        /// <param name="contact">The other member</param>
        /// <param name="content">The content of the message to delete</param>
        /// <returns>The new conversation</returns>
        public async Task<Conversation> DeleteMessage(string username, string contact, string content)
        {
            Conversation conversation = await GetMessages(username, contact);
            if (conversation != null)
            {
                conversation.Messages.RemoveAll(m => m.Content.Equals(content));
                if (conversation.Messages.Count > 0)
                {
                    await _conversation.ReplaceOneAsync(c => c.Id.Equals(conversation.Id), conversation);
                    return conversation;
                }
                else
                {
                    await DeleteConversation(conversation.Id);
                    return null;
                }
            }
            return null;
        }
        /// <summary>Get all the conversations of one user</summary>
        /// <param name="username">The username</param>
        /// <returns>A list with all the conversations</returns>
        public async Task<List<Conversation>> GetConversations(string username)
        {
            IAsyncCursor<Conversation> task = await _conversation.FindAsync(c => c.Members.Contains(username));
            List<Conversation> conversations = await task.ToListAsync();
            return conversations;
        }
        /// <summary>Get all the messages of one conversation</summary>
        /// <param name="username">The username member</param>
        /// <param name="contact">The other member</param>
        /// <returns>The specific conversation</returns>
        public async Task<Conversation> GetMessages(string username, string contact)
        {
            List<Conversation> conversations = await GetConversations(username);
            if (conversations != null && conversations.Count > 0)
            {
                Conversation conversation = conversations.Find(c => c.Members.Contains(contact));
                return conversation;
            }
            return null;
        }
        /// <summary>Modified a existing message</summary>
        /// <param name="username">The user that makes the request</param>
        /// <param name="contact">The other member of the conversation</param>
        /// <param name="content">The old messgae</param>
        /// <param name="newContent">The new message</param>
        /// <returns>The new conversation</returns>
        public async Task<Conversation> ModifiedMessage(string username, string contact, string content, string newContent)
        {
            Conversation conversation = await GetMessages(username, contact);
            if (conversation != null)
            {
                if (conversation.Messages.Count > 0)
                {
                    int index = conversation.Messages.FindIndex(m => m.Content.Equals(newContent));
                    if (index != -1)
                    {
                        conversation.Messages[index].Content = newContent;
                        await _conversation.ReplaceOneAsync(c => c.Id.Equals(conversation.Id), conversation);
                        return conversation;
                    }
                }
            }
            return null;
        }
    }
}