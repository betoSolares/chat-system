using frontend_app.Helpers;
using frontend_app.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace frontend_app.Controllers
{
    public class ContactsController : Controller
    {
        [HttpGet]
        public ActionResult Find()
        {
            if (Session["username"] != null)
            {
                ViewBag.Error = "NO";
                ViewBag.Contacts = null;
                return View();
            }
            return RedirectToAction("LogIn", "Authentication");
        }
        
        [HttpPost]
        public ActionResult Find(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            (HttpResponseMessage result, Task<string> readTask) = new Requests<FindContact>().Post(Session["token"].ToString(),
                                                                                                  "contact/specific",
                                                                                                  findContact);
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
            (HttpResponseMessage result, Task<string> readTask) = new Requests<FindContact>().Post(Session["token"].ToString(),
                                                                                                  "contact/add",
                                                                                                  findContact);
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
            (HttpResponseMessage result, Task<string> readTask) = new Requests<FindContact>().Post(Session["token"].ToString(),
                                                                                                  "contact/remove",
                                                                                                  findContact);
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
            if (Session["username"] != null)
            {
                (HttpResponseMessage result, Task<string> readTask) = new Requests<int>().Get(Session["token"].ToString(), "contact/requests/" + Session["username"]);
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
            return RedirectToAction("LogIn", "Authentication");
        }
        
        [HttpPost]
        public ActionResult AcceptRequest(string username)
        {
            FindContact findContact = new FindContact()
            {
                Username = Session["username"].ToString(),
                Otheruser = username
            };
            (HttpResponseMessage result, Task<string> readTask) = new Requests<FindContact>().Post(Session["token"].ToString(),
                                                                                                  "contact/accept",
                                                                                                  findContact);
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
            (HttpResponseMessage result, Task<string> readTask) = new Requests<FindContact>().Post(Session["token"].ToString(),
                                                                                                  "contact/decline",
                                                                                                  findContact);
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
            if (Session["username"] != null)
            {
                (HttpResponseMessage result, Task<string> readTask) = new Requests<int>().Get(Session["token"].ToString(), "contact/mycontacts/" + Session["username"]);
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
            return RedirectToAction("LogIn", "Authentication");
        }
    }
}