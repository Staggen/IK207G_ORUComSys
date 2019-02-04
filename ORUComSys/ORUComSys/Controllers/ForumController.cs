using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace ORUComSys.Controllers {
    [Authorize]
    public class ForumController : Controller {
        private PostRepository postRepository;
        private ProfileRepository profileRepository;

        public ForumController() {
            ApplicationDbContext context = new ApplicationDbContext();
            postRepository = new PostRepository(context);
            profileRepository = new ProfileRepository(context);
        }

        public ActionResult Index() { // Select which forum you wish to enter
            return View();
        }

        public ActionResult Formal() { // Formal forum
            return View(ConvertPostsToViewModels(ForumType.Formal));
        }

        public ActionResult Informal() { // Informal forum
            return View(ConvertPostsToViewModels(ForumType.Informal));
        }

        public PartialViewResult UpdatePosts(string Id) {
            // Check if the string argument is formal. If it is, set type to Formal, else set it to Informal.
            ForumType type = string.Equals(Id, "Formal", System.StringComparison.OrdinalIgnoreCase) ? ForumType.Formal : ForumType.Informal; 
            return PartialView("_ForumPosts", ConvertPostsToViewModels(type));
        }

        public PostViewModels ConvertPostsToViewModels(ForumType type) {
            var allPosts = postRepository.GetAllPostsByForumType(type);

            var postViewModel = new PostViewModels {
                CurrentUser = profileRepository.Get(User.Identity.GetUserId()),
                PostList = allPosts
            };

            return postViewModel;
        }
    }
}