using Datalayer.Models;
using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Datalayer.Repositories;
using System.IO;
using System.Web.Http;
using System.Linq;

namespace ORUComSys.Controllers {
    public class AjaxApiController : ApiController {
        private ProfileRepository profileRepository;
        private UserRepository userRepository;
        private PostRepository postRepository;

        public AjaxApiController() {
            ApplicationDbContext context = new ApplicationDbContext();
            profileRepository = new ProfileRepository(context);
            userRepository = new UserRepository(context);
            postRepository = new PostRepository(context);
        }

        [HttpDelete]
        public void DeletePost(int id) {
            postRepository.Remove(id);
            postRepository.Save();
        }
    }
}