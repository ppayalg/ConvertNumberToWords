using System.Web.Mvc;

namespace TestAkQA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Test AKQA";

            return View();
        }

    }
}

