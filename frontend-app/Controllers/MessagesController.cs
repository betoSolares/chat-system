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
            client.Dispose();
            
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                JObject json = JObject.Parse(readTask.Result);
                JArray array = (JArray)json["contacts"];
                List<Contact> contacts = array.ToObject<List<Contact>>();
                ViewBag.ERROR = "NO";
                ViewBag.Contacts = contacts;
                return View();
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.ERROR = "YES";
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