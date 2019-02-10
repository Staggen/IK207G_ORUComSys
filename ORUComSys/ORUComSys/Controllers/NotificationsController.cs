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
        private MeetingProposalRepository meetingProposalRepository;
        private MeetingInviteeRepository meetingInviteeRepository;
        private PostRepository postRepository;
        private ProfileRepository profileRepository;

        public NotificationsController() {
            ApplicationDbContext context = new ApplicationDbContext();
            followingCategoryRepository = new FollowingCategoryRepository(context);
            meetingProposalRepository = new MeetingProposalRepository(context);
            meetingInviteeRepository = new MeetingInviteeRepository(context);
            postRepository = new PostRepository(context);
            profileRepository = new ProfileRepository(context);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetNumberOfNotifications() {
            string currentUserId = User.Identity.GetUserId();
            ProfileModels profile = profileRepository.Get(currentUserId);

            List<FollowingCategoryModels> Categories = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId);
            List<MeetingInviteeModels> ProposalInvites = meetingInviteeRepository.GetAllMeetingProposalsForUserId(currentUserId);

            List<PostModels> CombinedPostList = null;
            CombinedPostList.AddRange(postRepository.GetAllPostsInCategorySinceLastUserLoginByUserId(currentUserId, Categories, profile.LastLogin));
            CombinedPostList.AddRange(postRepository.GetAllPostsFromFollowedUserByUserId(currentUserId, profile.LastLogin));

            List<PostModels> DistinctCombinedPostList = CombinedPostList.Distinct().ToList();

            // actual meetings (that one can respond to) should get fetched here too, once they are included.

            return Json(new { Number = ProposalInvites.Count + DistinctCombinedPostList.Count });
        }

        public PartialViewResult GetNotificationsContent() {
            string currentUserId = User.Identity.GetUserId();
            ProfileModels profile = profileRepository.Get(currentUserId);

            List<FollowingCategoryModels> Categories = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId);

            List<PostModels> CombinedPostList = null;
            CombinedPostList.AddRange(postRepository.GetAllPostsInCategorySinceLastUserLoginByUserId(currentUserId, Categories, profile.LastLogin));
            CombinedPostList.AddRange(postRepository.GetAllPostsFromFollowedUserByUserId(currentUserId, profile.LastLogin));

            NotificationsViewModels notifications = new NotificationsViewModels {
                Invites = meetingInviteeRepository.GetAllMeetingProposalsForUserId(currentUserId),
                Posts = CombinedPostList.Distinct().ToList()
            };

            return PartialView("_Notifications", notifications);
        }
    }
}