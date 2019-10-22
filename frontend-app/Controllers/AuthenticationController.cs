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
    }
}