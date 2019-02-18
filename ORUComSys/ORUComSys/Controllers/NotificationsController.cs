using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    public class NotificationsController : Controller {
        private FollowingCategoryRepository followingCategoryRepository;
        private MeetingInviteRepository meetingInviteRepository;
        private ProfileRepository profileRepository;
        private PostRepository postRepository;

        public NotificationsController() {
            ApplicationDbContext context = new ApplicationDbContext();
            followingCategoryRepository = new FollowingCategoryRepository(context);
            meetingInviteRepository = new MeetingInviteRepository(context);
            profileRepository = new ProfileRepository(context);
            postRepository = new PostRepository(context);
        }
        
        [HttpPost]
        public ActionResult GetNumberOfNotifications() {
            string currentUserId = User.Identity.GetUserId();

            ProfileModels profile = profileRepository.Get(currentUserId);
            // Get all meeting invites for the profile id, then select only the ones which have not been accepted.
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllMeetingInvitesForProfileId(currentUserId).Where((i) => !i.Accepted).ToList();
            List<FollowingCategoryModels> fc = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId);
            List<PostModels> newPosts = postRepository.GetAllPostsInCategorySinceLastUserLoginByUserId(fc, profile.LastLogin);

            return Json(new { Number = meetingInvites.Count + newPosts.Count });
        }

        [HttpGet]
        public PartialViewResult GetNotificationsContent() {
            string currentUserId = User.Identity.GetUserId();

            ProfileModels profile = profileRepository.Get(currentUserId);
            // Get all meeting invites for the profile id, then select only the ones which have not been accepted.
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllMeetingInvitesForProfileId(currentUserId).Where((i) => !i.Accepted).ToList();
            List<FollowingCategoryModels> fc = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId);
            List<PostModels> newPosts = postRepository.GetAllPostsInCategorySinceLastUserLoginByUserId(fc, profile.LastLogin).OrderByDescending((p) => p.PostDateTime).ToList();
            List<ProfileModels> profiles = new List<ProfileModels>();
            foreach(var post in newPosts) {
                profiles.Add(profileRepository.Get(post.PostFromId));
            }

            NotificationsViewModels notifications = new NotificationsViewModels {
                Invites = meetingInvites.OrderByDescending((i) => i.InviteDateTime).ToList(),
                Posts = newPosts,
                PostFrom = profiles
            };

            return PartialView("_Notifications", notifications);
        }
    }
}