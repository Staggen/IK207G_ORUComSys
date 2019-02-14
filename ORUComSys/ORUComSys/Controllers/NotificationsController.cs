using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    public class NotificationsController : Controller {
        private MeetingInviteRepository meetingInviteRepository;
        private ProfileRepository profileRepository;

        public NotificationsController() {
            ApplicationDbContext context = new ApplicationDbContext();
            meetingInviteRepository = new MeetingInviteRepository(context);
            profileRepository = new ProfileRepository(context);
        }
        
        [HttpPost]
        public ActionResult GetNumberOfNotifications() {
            string currentUserId = User.Identity.GetUserId();

            ProfileModels profile = profileRepository.Get(currentUserId);
            // Get all meeting invites for the profile id, then select only the ones which have not been accepted.
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllMeetingInvitesForProfileId(currentUserId).Where((i) => !i.Accepted).ToList();

            return Json(new { Number = meetingInvites.Count });
        }

        [HttpGet]
        public PartialViewResult GetNotificationsContent() {
            string currentUserId = User.Identity.GetUserId();

            ProfileModels profile = profileRepository.Get(currentUserId);
            // Get all meeting invites for the profile id, then select only the ones which have not been accepted.
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllMeetingInvitesForProfileId(currentUserId).Where((i) => !i.Accepted).ToList();

            NotificationsViewModels notifications = new NotificationsViewModels {
                Invites = meetingInvites.OrderByDescending((i) => i.InviteDateTime).ToList(),
            };

            return PartialView("_Notifications", notifications);
        }
    }
}