using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
    public class CalendarController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}