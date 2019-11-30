using frontend_app.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.GetAsync("contact/mycontacts/" + Session["username"]);
            response.Wait();

            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                Task<HttpResponseMessage> newResponse = client.GetAsync("conversations/conversations/" + Session["username"]);
                newResponse.Wait();
                client.Dispose();

                JObject json = JObject.Parse(readTask.Result);
                JArray array = (JArray)json["contacts"];
                List<Contact> contacts = array.ToObject<List<Contact>>();

                HttpResponseMessage newResult = newResponse.Result;
                Task<string> newTask = newResult.Content.ReadAsStringAsync();
                newTask.Wait();
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
                Content = content
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("conversations/send", message);
            response.Wait();
            client.Dispose();
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
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
                Content = content
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("conversations/delete", message);
            response.Wait();
            client.Dispose();
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
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
                Content = content,
                NewContent = newContent
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PutAsJsonAsync("conversations/modify", message);
            response.Wait();
            client.Dispose();
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
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
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.GetAsync("conversations/messages/" + Session["username"].ToString() + "/" + username);
            response.Wait();

            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();

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
                        Content = message.Content
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
    }
}