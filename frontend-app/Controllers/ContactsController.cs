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
    public class ContactsController : Controller
    {
        // GET: Contacts
        [HttpGet]
        public ActionResult Find()
        {
            ViewBag.Error = "NO";
            ViewBag.Contacts = null;
            return View();
        }
        
        [HttpPost]
        public ActionResult Find(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("contact/specific", findContact);
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
                contacts.RemoveAll(x => x.Username.Equals(Session["username"].ToString()));
                ViewBag.Error = "NO";
                ViewBag.Contacts = contacts;
                return View();
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "YES";
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
        
        [HttpPost]
        public ActionResult AddRequest(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("contact/add", findContact);
            response.Wait();
            client.Dispose();
            
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                ViewBag.Error = "NO";
                return View("Find");
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "YES";
                    return View("Find");
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
        
        [HttpPost]
        public ActionResult RemoveRequest(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("contact/remove", findContact);
            response.Wait();
            client.Dispose();
            
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                ViewBag.Error = "NO";
                return View("Find");
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "YES";
                    return View("Find");
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
        
        [HttpGet]
        public ActionResult Requests()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.GetAsync("contact/requests/" + Session["username"]);
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
        
        [HttpPost]
        public ActionResult AcceptRequest(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("contact/accept", findContact);
            response.Wait();
            client.Dispose();
            
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Requests");
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "YES";
                    return RedirectToAction("Requests");
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
        
        [HttpPost]
        public ActionResult DeleteRequest(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("contact/decline", findContact);
            response.Wait();
            client.Dispose();
            
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Requests");
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "YES";
                    return RedirectToAction("Requests");
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
        
        [HttpGet]
        public ActionResult MyContacts()
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