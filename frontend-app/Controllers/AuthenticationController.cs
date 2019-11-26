using frontend_app.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace frontend_app.Controllers
{
    public class AuthenticationController : Controller
    {
        // Sign Up form
        // GET: /SignUp, /Authentication/SignUp
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        // Try to create a new account
        [HttpPost]
        public ActionResult SignUp(string name, string lastname, string email, string username, string password)
        {
            SignUp signUp = new SignUp()
            {
                GivenName = name,
                FamilyName = lastname,
                Email = email,
                Username = username,
                Password = password
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("account/signup", signUp);
            response.Wait();
            client.Dispose();

            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                var definition = new { token = "" };
                string token = JsonConvert.DeserializeAnonymousType(readTask.Result, definition).token;
                Session["username"] = username;
                Session["token"] = token;
                return RedirectToAction("Inbox", "Messages");
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.Conflict)
                {
                    var definition = new { error = "" };
                    string error = JsonConvert.DeserializeAnonymousType(readTask.Result, definition).error;
                    ViewBag.Message = "ERROR";
                    ViewBag.Error = error;
                    return View();
                }
                else
                {
                    ViewBag.Message = "ERROR";
                    ViewBag.Error = "An error ocurred, try later.";
                    return View();
                }
            }
        }

        // Log In form
        // GET: /LogIn, /Authentication/LogIn
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        
        // Try to access to the chat
        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            LogIn logIn = new LogIn()
            {
                Username = username,
                Password = password
            };
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            
            Task<HttpResponseMessage> response = client.PostAsJsonAsync("account/login", logIn);
            response.Wait();
            client.Dispose();
            
            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            if (result.IsSuccessStatusCode)
            {
                var definition = new { token = "" };
                string token = JsonConvert.DeserializeAnonymousType(readTask.Result, definition).token;
                Session["username"] = username;
                Session["token"] = token;
                return RedirectToAction("Inbox", "Messages");
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var definition = new { error = "" };
                    string error = JsonConvert.DeserializeAnonymousType(readTask.Result, definition).error;
                    ViewBag.Message = "ERROR";
                    ViewBag.Error = error;
                    return View();
                }
                else
                {
                    ViewBag.Message = "ERROR";
                    ViewBag.Error = "An error ocurred, try later.";
                    return View();
                }
            }
        }
    }
}