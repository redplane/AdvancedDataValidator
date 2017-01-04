using System.Web.Mvc;

namespace ToolKit.DataAnnotation.Test.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Render index page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}