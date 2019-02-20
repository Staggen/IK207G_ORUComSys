using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize(Roles = "Profiled")]
    public class MeetingController : Controller {
        private MeetingRepository meetingRepository;
        private MeetingInviteRepository meetingInviteRepository;
        private ProfileRepository profileRepository;
        private UserRepository userRepository;

        public MeetingController() {
            ApplicationDbContext context = new ApplicationDbContext();
            meetingRepository = new MeetingRepository(context);
            meetingInviteRepository = new MeetingInviteRepository(context);
            profileRepository = new ProfileRepository(context);
            userRepository = new UserRepository(context);
        }

        public ActionResult Index() {
            string currentUserId = User.Identity.GetUserId();
            List<MeetingInviteModels> myMeetingInvites = meetingInviteRepository.GetAllInvitesForProfileId(currentUserId);
            List<int> myMeetingIds = myMeetingInvites.Where(meetingInvite => meetingInvite.ProfileId.Equals(currentUserId)).Select(meetingInvite => meetingInvite.MeetingId).ToList();
            List<MeetingModels> myMeetings = meetingRepository.GetMeetingsByMeetingIds(myMeetingIds);
            List<MeetingModels> myCreatedMeetings = meetingRepository.GetAllMeetingsByCreatorId(currentUserId);
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllInvitesByMeetingIds(myMeetingIds).Where(invite => invite.Accepted).ToList();

            MeetingViewModels model = new MeetingViewModels {
                ProfileId = currentUserId,
                MyMeetings = myMeetings,
                MyCreatedMeetings = myCreatedMeetings,
                MeetingInvites = meetingInvites,
                MyMeetingInvites = myMeetingInvites
            };
            return View(model);
        }

        public ActionResult CreateMeeting() {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMeeting(MeetingModels meeting) {
            if(ModelState.IsValid) {
                string currentUserId = User.Identity.GetUserId();
                MeetingModels model = new MeetingModels {
                    CreatorId = currentUserId,
                    Title = meeting.Title,
                    Description = meeting.Description,
                    MeetingDateTime = meeting.MeetingDateTime,
                    Location = meeting.Location,
                    Type = meeting.Type
                };
                meetingRepository.Add(model); // Add meeting
                meetingRepository.Save();

                MeetingInviteModels inviteModel = new MeetingInviteModels {
                    MeetingId = model.Id,
                    ProfileId = currentUserId,
                    InviteDateTime = DateTime.Now,
                    Accepted = true
                };
                meetingInviteRepository.Add(inviteModel); // Invite yourself to the meeting (calendar purposes)
                meetingInviteRepository.Save();

                return RedirectToAction("MeetingInvitePeople", new { id = model.Id });
            }
            return View();
        }

        public ActionResult EditMeeting(int id) {
            MeetingModels meeting = meetingRepository.Get(id);
            return View(meeting);
        }

        [HttpPost]
        public ActionResult EditMeeting(MeetingModels meeting) {
            if(!ModelState.IsValid) {
                return RedirectToAction("EditMeeting");
            }
            meeting.CreatorId = User.Identity.GetUserId();
            meetingRepository.Edit(meeting);
            meetingRepository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult MeetingInvitePeople(int Id) {
            List<ProfileModels> allProfiles = profileRepository.GetAllProfilesExceptCurrent(User.Identity.GetUserId());
            List<MeetingInviteModels> allInvites = meetingInviteRepository.GetAll();
            MeetingInviteViewModels inviteViewModel = new MeetingInviteViewModels {
                MeetingId = Id,
                Profiles = allProfiles.OrderBy(profile => profile.FirstName).ToList(),
                Invites = allInvites
            };
            return View(inviteViewModel);
        }

        [HttpPost]
        public ActionResult AddMeetingInvite(InviteViewModels invite) {
            if(ModelState.IsValid) {
                MeetingInviteModels inviteModel = new MeetingInviteModels {
                    MeetingId = invite.MeetingId,
                    ProfileId = invite.ProfileId,
                    InviteDateTime = DateTime.Now,
                    Accepted = false
                };
                meetingInviteRepository.Add(inviteModel);
                meetingInviteRepository.Save();
                // Send notification email
                EmailViewModels emailModel = new EmailViewModels {
                    Sender = profileRepository.Get(User.Identity.GetUserId()),
                    Recipient = profileRepository.Get(invite.ProfileId),
                    Meeting = meetingRepository.Get(invite.MeetingId)
                };
                string viewPath = "~/Views/Meeting/NewMeetingNotificationEmail.cshtml";
                string recipient = userRepository.GetEmailByUserId(invite.ProfileId);
                string subject = "New Meeting Invite - ORUComSys";
                EmailSupport.SendNotificationEmail(ControllerContext, viewPath, emailModel, recipient, subject);

                return Json(new { result = true });
            }

            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult RemoveMeetingInvite(InviteViewModels invite) {
            if(ModelState.IsValid) {
                MeetingInviteModels meetingInvite = meetingInviteRepository.GetInvite(invite.ProfileId, invite.MeetingId);
                meetingInviteRepository.Remove(meetingInvite.Id);
                meetingInviteRepository.Save();
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult AcceptMeetingInvite(int id) {
            MeetingInviteModels meetingInvite = meetingInviteRepository.GetInvite(User.Identity.GetUserId(), id);
            meetingInvite.Accepted = true;
            meetingInviteRepository.Edit(meetingInvite);
            meetingInviteRepository.Save();
            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult DeclineMeetingInvite(int id) {
            MeetingInviteModels meetingInvite = meetingInviteRepository.GetInvite(User.Identity.GetUserId(), id);
            meetingInviteRepository.Remove(meetingInvite.Id);
            meetingInviteRepository.Save();
            return Json(new { result = true });
        }

        [HttpPost]
        public PartialViewResult GetParticipantsContent(int id) {
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllInvitesByMeetingId(id).Where(invite => invite.Accepted).ToList();
            return PartialView("_ParticipantsList", meetingInvites);
        }
    }
}