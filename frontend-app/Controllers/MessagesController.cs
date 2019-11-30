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
                HttpResponseMessage newResult = newResponse.Result;
                Task<string> newTask = newResult.Content.ReadAsStringAsync();
                newTask.Wait();
                if (newResult.IsSuccessStatusCode)
                {
                    JObject json = JObject.Parse(newTask.Result);
                    JArray array = (JArray)json["conversations"];
                    List<Conversation> conversations = array.ToObject<List<Conversation>>();
                    ViewBag.Error = "NO";
                    ViewBag.Conversations = conversations;
                    return View();
                }
                else if (newResult.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "Conversations";
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
        public ActionResult SendMessage(string username, string content)
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
                ChatHub.BroadcastData();
                return RedirectToAction("Inbox");
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
                ChatHub.BroadcastData();
                return RedirectToAction("Inbox");
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
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("conversations/modify", message);
            response.Wait();
            client.Dispose();
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                ChatHub.BroadcastData();
                return RedirectToAction("Inbox");
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