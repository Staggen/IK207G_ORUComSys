using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
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
            if(!profileRepository.IfProfileExists(currentUserId)) {
                return Json(new { Number = 0 });
            }
            ProfileModels profile = profileRepository.Get(currentUserId);
            // Get all meeting invites for the profile id, then select only the ones which have not been accepted.
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllInvitesForProfileId(currentUserId).Where(meetingInvite => !meetingInvite.Accepted).ToList();
            List<int> followedCategoryIds = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId).Select(followedCategory => followedCategory.CategoryId).ToList();
            List<PostModels> newPosts = postRepository.GetAllPostsInFollowedCategoriesSinceLastLogout(followedCategoryIds, profile.LastLogout, currentUserId);
            return Json(new { Number = meetingInvites.Count + newPosts.Count });

        }

        [HttpGet]
        public PartialViewResult GetNotificationsContent() {
            string currentUserId = User.Identity.GetUserId();
            if(!profileRepository.IfProfileExists(currentUserId)) {
                return PartialView("_NoProfile_Notifications");
            }
            ProfileModels profile = profileRepository.Get(currentUserId);
            // Get all meeting invites for the profile id, then select only the ones which have not been accepted.
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllInvitesForProfileId(currentUserId).Where(meetingInvite => !meetingInvite.Accepted).ToList();
            List<int> followedCategoryIds = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId).Select(followedCategory => followedCategory.CategoryId).ToList();
            List<PostModels> newPosts = postRepository.GetAllPostsInFollowedCategoriesSinceLastLogout(followedCategoryIds, profile.LastLogout, currentUserId).OrderByDescending(post => post.PostDateTime).ToList();
            List<ProfileModels> profiles = new List<ProfileModels>();
            foreach(var post in newPosts) {
                profiles.Add(profileRepository.Get(post.PostFromId));
            }
            NotificationsViewModels notifications = new NotificationsViewModels {
                Invites = meetingInvites.OrderByDescending(meetingInvite => meetingInvite.InviteDateTime).ToList(),
                Posts = newPosts,
                PostFrom = profiles.Distinct().ToList()
            };
            return PartialView("_Notifications", notifications);
        }
    }
}