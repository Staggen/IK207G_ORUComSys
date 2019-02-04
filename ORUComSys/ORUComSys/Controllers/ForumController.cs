using Datalayer.Repositories;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    public class ForumController : Controller {
        private PostRepository postRepository;

        public ForumController() {
            ApplicationDbContext context = new ApplicationDbContext();
            postRepository = new PostRepository(context);
        }

        public ActionResult Index() { // Select which forum you wish to enter
            return View();
        }

        public ActionResult Formal() { // Formal forum
            return View();
        }

        public ActionResult Informal() { // Informal forum
            return View();
        }
    }
}