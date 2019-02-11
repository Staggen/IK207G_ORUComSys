using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
    public class SearchController : Controller {
        private ProfileRepository profileRepository;

        public SearchController() {
            ApplicationDbContext context = new ApplicationDbContext();
            profileRepository = new ProfileRepository(context);
        }

        public ActionResult Index() {
            List<ProfileModels> allProfiles = profileRepository.GetAllProfilesExceptCurrent(User.Identity.GetUserId());
            
            return View(allProfiles.OrderBy((p) => p.FirstName)); // Sort list to have users appear in the search window by match percentage by default
        }
    }
}