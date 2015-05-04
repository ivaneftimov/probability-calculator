using System.Web.Mvc;

namespace ProbabilityCalculator.Controllers
{
    public class AppController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
