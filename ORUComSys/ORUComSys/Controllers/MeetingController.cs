using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
    public class MeetingController : Controller {
        private MeetingRepository meetingRepository;
        //private MeetingProposalRepository meetingProposalRepository;
        private MeetingInviteeRepository meetingInviteeRepository;
        private ProfileRepository profileRepository;
        //private ProposalInviteeRepository proposalInviteeRepository;

        public MeetingController() {
            ApplicationDbContext context = new ApplicationDbContext();
            meetingRepository = new MeetingRepository(context);
            //meetingProposalRepository = new MeetingProposalRepository(context);
            meetingInviteeRepository = new MeetingInviteeRepository(context);
            profileRepository = new ProfileRepository(context);
            //proposalInviteeRepository = new ProposalInviteeRepository(context);
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult CreateMeeting() {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMeeting(MeetingModels meeting) {
            if (ModelState.IsValid) {
                MeetingModels model = new MeetingModels {
                    CreatorId = User.Identity.GetUserId(),
                    Title = meeting.Title,
                    Description = meeting.Description,
                    MeetingDateTime = meeting.MeetingDateTime,
                    Location = meeting.Location,
                    Type = meeting.Type
                };
                meetingRepository.Add(model); // Add meeting
                meetingRepository.Save();

                MeetingInviteeModels inviteModel = new MeetingInviteeModels {
                    MeetingId = model.Id,
                    ProfileId = User.Identity.GetUserId(),
                    MeetingAccepted = true
                };
                meetingInviteeRepository.Add(inviteModel); // Invite yourself to the meeting (calendar purposes)
                meetingInviteeRepository.Save();

                return RedirectToAction("MeetingInvitePeople", new { id = model.Id });
            }
            return View();
        }

        public ActionResult MeetingInvitePeople(int Id) {
            ViewBag.MeetingId = Id;
            List<ProfileModels> allProfiles = profileRepository.GetAllProfilesExceptCurrent(User.Identity.GetUserId());
            return View(allProfiles.OrderBy((p) => p.FirstName));
        }

        public ActionResult ProposeMeeting() {
            return View();
        }

        [HttpPost]
        public ActionResult ProposeMeeting(MeetingProposalModels proposal) {
            if (ModelState.IsValid) {
                
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddMeetingInvite(InviteViewModel invite) {
            if (ModelState.IsValid) {
                MeetingInviteeModels model = new MeetingInviteeModels {
                    MeetingId = invite.MeetingId,
                    ProfileId = invite.ProfileId
                };
                meetingInviteeRepository.Add(model);
                meetingInviteeRepository.Save();
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult RemoveMeetingInvite(InviteViewModel invite) {
            if (ModelState.IsValid) {
                MeetingInviteeModels model = meetingInviteeRepository.GetMeetingInviteByUserIdAndMeetingId(invite.ProfileId, invite.MeetingId);
                meetingInviteeRepository.Remove(model.Id);
                meetingInviteeRepository.Save();
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }
    }
}