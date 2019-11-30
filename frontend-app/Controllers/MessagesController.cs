using external_process.Encryption;
using frontend_app.Helpers;
using frontend_app.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace frontend_app.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Messages
        [HttpGet]
        public ActionResult Inbox()
        {
            (HttpResponseMessage result, Task<string> readTask) = new Requests<int>().Get(Session["token"].ToString(), "contact/mycontacts/" + Session["username"]);
            if (result.IsSuccessStatusCode)
            {
                (HttpResponseMessage newResult, Task<string> newTask) = new Requests<int>().Get(Session["token"].ToString(), "conversations/conversations/" + Session["username"]);

                JObject json = JObject.Parse(readTask.Result);
                JArray array = (JArray)json["contacts"];
                List<Contact> contacts = array.ToObject<List<Contact>>();

                if (newResult.IsSuccessStatusCode)
                {
                    json = JObject.Parse(newTask.Result);
                    array = (JArray)json["conversations"];
                    List<Conversation> conversations = array.ToObject<List<Conversation>>();
                    ViewBag.Error = "NO";
                    ViewBag.Contacts = contacts;
                    ViewBag.Conversations = conversations;
                    return View();
                }
                else if (newResult.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "Conversations";
                    ViewBag.Contacts = contacts;
                    return View();
                }
                else if (newResult.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return View("~/Views/Shared/Unauthorize.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "Contacts";
                    return View();
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return View("~/Views/Shared/Unauthorize.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }

        // Send a new message
        [HttpPost]
        public ActionResult SendMessage(string username, string content, string type)
        {
            MessageToSent message = new MessageToSent()
            {
                From = Session["username"].ToString(),
                To = username,
                Content = EncryptText(content)
            };
            (HttpResponseMessage result, Task<string> readTask) = new Requests<MessageToSent>().Post(Session["token"].ToString(),
                                                                                                  "conversations/send",
                                                                                                  message);
            if (result.IsSuccessStatusCode)
            {
                if (type.Equals("new"))
                    return RedirectToAction("Inbox");
                else
                    return RedirectToAction("GetMessages", new { username });
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "NOT FOUND";
                    return View();
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return View("~/Views/Shared/Unauthorize.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }

        // Delete an existing message
        [HttpPost]
        public ActionResult DeleteMessage(string username, string content)
        {
            MessageToSent message = new MessageToSent()
            {
                From = Session["username"].ToString(),
                To = username,
                Content = EncryptText(content)
            };
            (HttpResponseMessage result, Task<string> readTask) = new Requests<MessageToSent>().Post(Session["token"].ToString(),
                                                                                                  "conversations/delete",
                                                                                                  message);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("GetMessages", new { username });
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "NOT FOUND";
                    return View();
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return View("~/Views/Shared/Unauthorize.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }

        // Modify an existing message
        [HttpPost]
        public ActionResult ModifyMessage(string username, string content, string newContent)
        {
            MessageToSent message = new MessageToSent()
            {
                From = Session["username"].ToString(),
                To = username,
                Content = EncryptText(content),
                NewContent = EncryptText(newContent)
            };
            (HttpResponseMessage result, Task<string> readTask) = new Requests<MessageToSent>().Put(Session["token"].ToString(),
                                                                                                  "conversations/modify",
                                                                                                  message);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("GetMessages", new { username });
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "NOT FOUND";
                    return RedirectToAction("GetMessages"); ;
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return View("~/Views/Shared/Unauthorize.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }

        // Get all the messages
        [HttpGet]
        public ActionResult GetMessages(string username)
        {
            (HttpResponseMessage result, Task<string> readTask) = new Requests<int>().Get(Session["token"].ToString(), "conversations/messages/" + Session["username"].ToString() + "/" + username);
            if (result.IsSuccessStatusCode)
            {
                JObject json = JObject.Parse(readTask.Result);
                JArray array = (JArray)json["messages"];
                List<MessageReceive> messages = array.ToObject<List<MessageReceive>>();
                List<Message> _messages = new List<Message>();
                foreach(MessageReceive message in messages)
                {
                    Message newMessage = new Message()
                    {
                        From = message.From,
                        Path = message.Path,
                        IsFile = message.IsFile,
                        Content = DecryptText(message.Content)
                    };
                    if (message.From.Equals(Session["username"].ToString()))
                        newMessage.IsFromMe = true;
                    else
                        newMessage.IsFromMe = false;
                    _messages.Add(newMessage);
                }
                ViewBag.Error = "no";
                ViewBag.ContactUser = username;
                ViewBag.Messages = _messages;
                return View();
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "NOT FOUND";
                    return View();
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return View("~/Views/Shared/Unauthorize.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }

        /// <summary>Encrypt the text</summary>
        /// <param name="text">the text to encrypt</param>
        /// <returns>The new text</returns>
        private string EncryptText(string text)
        {
            SDES sdes = new SDES();
            string newText = string.Empty;
            foreach (char character in text)
            {
                byte myByte = Convert.ToByte(character);
                byte encrypted = sdes.Encrypt(myByte, 899);
                newText += Convert.ToChar(encrypted);
            }
            return newText;
        }

        /// <summary>Decrypt the text</summary>
        /// <param name="text">the text to decrypt</param>
        /// <returns>The new text</returns>
        private string DecryptText(string text)
        {
            SDES sdes = new SDES();
            string newText = string.Empty;
            foreach (char character in text)
            {
                byte myByte = Convert.ToByte(character);
                byte decrypt = sdes.Decrypt(myByte, 899);
                newText += Convert.ToChar(decrypt);
            }
            return newText;
        }
    }
}