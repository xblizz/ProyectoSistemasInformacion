using System.Web.Helpers;
using System.Web.Mvc;
using SS.Core.Entities;
using SS.Core.Security;
using SS.Core.Security.Authorization;

namespace SistemaSeguridad.Controllers
{
    public class HomeController : Controller
    {
        //static readonly ConorContainer db = new ConorContainer();
        [RequireAuthorization]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return  View("index");
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menu = Security.GetMenu4ThisUser("");
            return PartialView("_Menu", menu);
        }
        public ActionResult About()
        { 
            
            return View();
        }
    }
}
