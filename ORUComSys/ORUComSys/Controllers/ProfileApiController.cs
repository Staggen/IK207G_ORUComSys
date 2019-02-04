using Datalayer.Models;
using Datalayer.Repositories;
using System.Web.Http;

namespace ORUComSys.Controllers {
    public class ProfileApiController : ApiController {
        private ProfileRepository profileRepository;

        public ProfileApiController() {
            ApplicationDbContext context = new ApplicationDbContext();
            profileRepository = new ProfileRepository(context);
        }

        [HttpPost]
        public void MakeAdmin(string userId) {
            ProfileModels profile = profileRepository.Get(userId);
            profile.IsAdmin = true;
            profileRepository.Edit(profile);
            profileRepository.Save();
        }
    }
}