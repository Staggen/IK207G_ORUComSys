using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Extensions;
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
            if(!ModelState.IsValid) {
                return RedirectToAction("CreateMeeting");
            }
            string currentUserId = User.Identity.GetUserId();
            // Fill out the data missing from the submitted model
            meeting.CreatorId = currentUserId;
            meetingRepository.Add(meeting); // Add meeting
            meetingRepository.Save();
            // Get latest added meeting so you have access to the meetings ID
            MeetingModels addedMeeting = meetingRepository.GetLastMeetingCreatedByProfileId(currentUserId);
            // Invite yourself and accept this invite for calendar purposes
            MeetingInviteModels inviteModel = new MeetingInviteModels {
                MeetingId = addedMeeting.Id,
                ProfileId = currentUserId,
                InviteDateTime = DateTime.Now,
                Accepted = true
            };
            meetingInviteRepository.Add(inviteModel);
            // Invite specific people if the meeting is not public
            if(addedMeeting.Type != MeetingType.Public) {
                meetingInviteRepository.Save();
                return RedirectToAction("MeetingInvitePeople", new { id = addedMeeting.Id });
            }
            // Invite everyone (except yourself) if the meeting is public. You have already invited yourself
            List<ProfileModels> exceptCurrent = profileRepository.GetAllProfilesExceptCurrent(currentUserId);
            foreach(ProfileModels profile in exceptCurrent) {
                inviteModel = new MeetingInviteModels {
                    MeetingId = addedMeeting.Id,
                    ProfileId = profile.Id,
                    InviteDateTime = DateTime.Now,
                    Accepted = false
                };
                meetingInviteRepository.Add(inviteModel);
                // Send notification email
                EmailViewModels emailModel = new EmailViewModels {
                    Sender = profileRepository.Get(currentUserId),
                    Recipient = profileRepository.Get(profile.Id),
                    Meeting = addedMeeting
                };
                string viewPath = "~/Views/Meeting/NewMeetingNotificationEmail.cshtml";
                string recipient = userRepository.GetEmailByUserId(profile.Id);
                string subject = "New Meeting Invite - ORUComSys";
                EmailSupport.SendNotificationEmail(ControllerContext, viewPath, emailModel, recipient, subject);
            }
            meetingInviteRepository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult EditMeeting(int id) {
            MeetingModels meeting = meetingRepository.Get(id);
            return View(meeting);
        }

        [HttpPost]
        public ActionResult EditMeeting(MeetingModels updates) {
            if(!ModelState.IsValid) {
                return RedirectToAction("EditMeeting");
            }
            // Get the existing meeting
            MeetingModels meeting = meetingRepository.Get(updates.Id);
            // If nothing changed
            if(
                meeting.Title.Equals(updates.Title) &&
                meeting.Description.Equals(updates.Description) &&
                meeting.Location.Equals(updates.Location) &&
                meeting.MeetingDateTime.Equals(updates.MeetingDateTime) &&
                meeting.Type == updates.Type
                ) {
                return RedirectToAction("Index");
            }
            meeting.Title = updates.Title;
            meeting.Description = updates.Description;
            meeting.Location = updates.Location;
            meeting.MeetingDateTime = updates.MeetingDateTime;
            meeting.Type = updates.Type;
            meetingRepository.Edit(meeting);
            meetingRepository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult MeetingInvitePeople(int Id) {
            List<ProfileModels> allProfiles = profileRepository.GetAllProfilesExceptCurrent(User.Identity.GetUserId()).OrderBy(profile => profile.FirstName).ToList();
            List<MeetingInviteModels> allInvites = meetingInviteRepository.GetAll();
            MeetingInviteViewModels inviteViewModel = new MeetingInviteViewModels {
                MeetingId = Id,
                Profiles = allProfiles,
                Invites = allInvites
            };
            return View(inviteViewModel);
        }

        [HttpPost]
        public ActionResult AddMeetingInvite(InviteViewModels invite) {
            if(!ModelState.IsValid) {
                return Json(new { result = false });
            }
            // Convert viewmodel to datalayer model
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

        [HttpPost]
        public PartialViewResult GetParticipantsContent(int id) {
            List<MeetingInviteModels> meetingInvites = meetingInviteRepository.GetAllInvitesByMeetingId(id).Where(invite => invite.Accepted).ToList();
            return PartialView("_ParticipantsList", meetingInvites);
        }

        [HttpPost]
        public ActionResult RemoveMeetingInvite(InviteViewModels invite) {
            if(!ModelState.IsValid) {
                return Json(new { result = false });
            }
            MeetingInviteModels meetingInvite = meetingInviteRepository.GetInvite(invite.ProfileId, invite.MeetingId);
            meetingInviteRepository.Remove(meetingInvite.Id);
            meetingInviteRepository.Save();
            return Json(new { result = true });
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
    }
}