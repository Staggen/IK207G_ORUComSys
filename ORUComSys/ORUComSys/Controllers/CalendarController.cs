using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize(Roles = "Profiled")]
    public class CalendarController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}