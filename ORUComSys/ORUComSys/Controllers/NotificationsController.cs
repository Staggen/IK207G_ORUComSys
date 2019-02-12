using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    public class NotificationsController : Controller {
        private MeetingInviteeRepository meetingInviteeRepository;
        private ProfileRepository profileRepository;

        public NotificationsController() {
            ApplicationDbContext context = new ApplicationDbContext();
            meetingInviteeRepository = new MeetingInviteeRepository(context);
            profileRepository = new ProfileRepository(context);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetNumberOfNotifications() {
            string currentUserId = User.Identity.GetUserId();
            ProfileModels profile = profileRepository.Get(currentUserId);

            List<MeetingInviteeModels> MeetingInvites = meetingInviteeRepository.GetAllMeetingInvitesForUserId(currentUserId);

            return Json(new { Number = MeetingInvites.Count });
        }

        public PartialViewResult GetNotificationsContent() {
            string currentUserId = User.Identity.GetUserId();
            ProfileModels profile = profileRepository.Get(currentUserId);

            NotificationsViewModels notifications = new NotificationsViewModels {
                Invites = meetingInviteeRepository.GetAllMeetingInvitesForUserId(currentUserId),
            };

            return PartialView("_Notifications", notifications);
        }
    }
}