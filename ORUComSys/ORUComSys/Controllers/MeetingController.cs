using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
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
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllMeetingInvitesForProfileId(currentUserId);
            List<int> myMeetingIds = meetingInvites.Where((m) => m.ProfileId.Equals(currentUserId)).Select((x) => x.MeetingId).ToList();
            List<MeetingModels> myCreatedMeetings = meetingRepository.GetAllMeetingsByCreatorId(currentUserId);
            List<MeetingModels> myMeetings = meetingRepository.GetListOfMeetingsByListOfMeetingIds(myMeetingIds);

            MeetingViewModels model = new MeetingViewModels {
                ProfileId = currentUserId,
                Invites = meetingInvites,
                MyCreatedMeetings = myCreatedMeetings,
                MyMeetings = myMeetings
            };
            return View(model);
        }

        public ActionResult CreateMeeting() {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMeeting(MeetingModels meeting) {
            if(ModelState.IsValid) {
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

                MeetingInviteModels inviteModel = new MeetingInviteModels {
                    MeetingId = model.Id,
                    ProfileId = User.Identity.GetUserId(),
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
            if(ModelState.IsValid) {
                meeting.CreatorId = User.Identity.GetUserId();
                meetingRepository.Edit(meeting);
                meetingRepository.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("EditMeeting");
        }

        public ActionResult MeetingInvitePeople(int Id) {
            List<ProfileModels> allProfiles = profileRepository.GetAllProfilesExceptCurrent(User.Identity.GetUserId());
            List<MeetingInviteModels> allInvites = meetingInviteRepository.GetAll();
            MeetingInviteViewModels inviteViewModel = new MeetingInviteViewModels {
                MeetingId = Id,
                Profiles = allProfiles.OrderBy((p) => p.FirstName).ToList(),
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

                string viewPath = "~/Views/Meeting/InlineNotificationEmail.cshtml";
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
                MeetingInviteModels meetingInvite = meetingInviteRepository.GetMeetingInvite(invite.ProfileId, invite.MeetingId);
                meetingInviteRepository.Remove(meetingInvite.Id);
                meetingInviteRepository.Save();
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult AcceptMeetingInvite(int id) {
            if(ModelState.IsValid) {
                MeetingInviteModels meetingInvite = meetingInviteRepository.GetMeetingInvite(User.Identity.GetUserId(), id);
                meetingInvite.Accepted = true;
                meetingInviteRepository.Edit(meetingInvite);
                meetingInviteRepository.Save();
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult DeclineMeetingInvite(int id) {
            if(ModelState.IsValid) {
                MeetingInviteModels meetingInvite = meetingInviteRepository.GetMeetingInvite(User.Identity.GetUserId(), id);
                meetingInviteRepository.Remove(meetingInvite.Id);
                meetingInviteRepository.Save();
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        public ActionResult NotificationEmail() { // JUST FOR TESTING PURPOSES
            ProfileModels sender = profileRepository.Get(User.Identity.GetUserId());
            ProfileModels recipient = profileRepository.Get("b83ed736-7ea9-43be-bc41-fd4f1851b7c3"); // = Elias Stagg's profile
            MeetingModels meeting = meetingRepository.Get(3);

            EmailViewModels emailModel = new EmailViewModels {
                Sender = sender,
                Recipient = recipient,
                Meeting = meeting
            };
            return View(emailModel);
        }

        public ActionResult InlineNotificationEmail() { // JUST FOR TESTING PURPOSES
            ProfileModels sender = profileRepository.Get(User.Identity.GetUserId());
            ProfileModels recipient = profileRepository.Get("b83ed736-7ea9-43be-bc41-fd4f1851b7c3"); // = Elias Stagg's profile
            MeetingModels meeting = meetingRepository.Get(3);

            EmailViewModels emailModel = new EmailViewModels {
                Sender = sender,
                Recipient = recipient,
                Meeting = meeting
            };
            return View(emailModel);
        }
    }
}