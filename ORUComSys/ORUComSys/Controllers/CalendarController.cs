using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize(Roles = "Profiled")]
    public class CalendarController : Controller {
        private MeetingRepository meetingRepository;
        private MeetingInviteRepository meetingInviteRepository;

        public CalendarController() {
            ApplicationDbContext context = new ApplicationDbContext();
            meetingRepository = new MeetingRepository(context);
            meetingInviteRepository = new MeetingInviteRepository(context);
        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult GetPublicCalendar() {
            List<MeetingModels> allPublicCalendarEntries = meetingRepository.GetMeetingsByMeetingType(MeetingType.Public);
            return Json(new { result = true, allEntries = allPublicCalendarEntries });
        }

        [HttpPost]
        public ActionResult GetUserSpecificCalendar() {
            string currentUserId = User.Identity.GetUserId();
            List<int> meetingIds = meetingInviteRepository.GetAllInvitesForProfileId(currentUserId).Where(invite => invite.Accepted).Select(invite => invite.MeetingId).ToList();
            List<MeetingModels> myMeetings = meetingRepository.GetMeetingsByMeetingIds(meetingIds).Where(meeting => meeting.Type != MeetingType.Public).ToList();
            return Json(new { result = true, allEntries = myMeetings });
        }
    }
}