using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize(Roles = "Profiled")]
    public class ProposalController : Controller {
        private ProfileRepository profileRepository;
        private UserRepository userRepository;
        private ProposedMeetingRepository proposedMeetingRepository;
        private ProposalInviteRepository proposalInviteRepository;

        public ProposalController() {
            ApplicationDbContext context = new ApplicationDbContext();
            userRepository = new UserRepository(context);
            profileRepository = new ProfileRepository(context);
            proposedMeetingRepository = new ProposedMeetingRepository(context);
            proposalInviteRepository = new ProposalInviteRepository(context);
        }

        public ActionResult Index() {
            string currentUserId = User.Identity.GetUserId();
            List<ProposalInviteModels> myProposalInvites = proposalInviteRepository.GetAllInvitesByProfileId(currentUserId);
            List<int> myProposalIds = myProposalInvites.Where(proposalInvite => proposalInvite.ProfileId.Equals(currentUserId)).Select(proposalInvite => proposalInvite.ProposalId).ToList();
            List<ProposedMeetingModels> myProposals = proposedMeetingRepository.GetProposedMeetingsByProposalIds(myProposalIds);
            List<ProposedMeetingModels> myCreatedProposals = proposedMeetingRepository.GetAllProposedMeetingsByHostId(currentUserId);
            List<ProposalInviteModels> proposalInvites = proposalInviteRepository.GetAllInvitesByProposalIds(myProposalIds);

            ProposedMeetingViewModels model = new ProposedMeetingViewModels {
                ProfileId = currentUserId,
                MyProposals = myProposals,
                MyCreatedProposals = myCreatedProposals,
                MyProposalInvites = myProposalInvites,
                ProposalInvites = proposalInvites
            };
            return View(model);
        }

        public ActionResult CreateProposal() {
            ProposedMeetingVesselModel test = new ProposedMeetingVesselModel();
            return View(test);
        }

        [HttpPost]
        public ActionResult CreateProposal(ProposedMeetingVesselModel vessel) {
            string currentUserId = User.Identity.GetUserId();
            ProposedMeetingModels proposal = new ProposedMeetingModels {
                HostId = currentUserId,
                Type = vessel.Type,
                Location = vessel.Location,
                Title = vessel.Title,
                Description = vessel.Description
            };
            proposedMeetingRepository.Add(proposal);
            proposedMeetingRepository.Save();
            // For every time (at least the first one), invite yourself.
            ProposalInviteModels firstInvite = new ProposalInviteModels {
                ProposalId = proposal.Id,
                ProfileId = currentUserId,
                NotificationDateTime = DateTime.Now,
                Accepted = true,
                ProposedDateTime = vessel.FirstTime
            };
            proposalInviteRepository.Add(firstInvite);
            if(vessel.SecondTime != null) {
                ProposalInviteModels secondInvite = new ProposalInviteModels {
                    ProposalId = proposal.Id,
                    ProfileId = currentUserId,
                    NotificationDateTime = DateTime.Now,
                    Accepted = true,
                    ProposedDateTime = (DateTime)vessel.SecondTime
                };
                proposalInviteRepository.Add(secondInvite);
            }
            if(vessel.ThirdTime != null) {
                ProposalInviteModels thirdInvite = new ProposalInviteModels {
                    ProposalId = proposal.Id,
                    ProfileId = currentUserId,
                    NotificationDateTime = DateTime.Now,
                    Accepted = true,
                    ProposedDateTime = (DateTime)vessel.ThirdTime
                };
                proposalInviteRepository.Add(thirdInvite);
            }
            proposalInviteRepository.Save();
            return RedirectToAction("ProposalInvitePeople", new { id = proposal.Id });
        }

        public ActionResult EditProposal(int Id) {
            ProposedMeetingModels proposal = proposedMeetingRepository.Get(Id);
            return View(proposal);
        }

        [HttpPost]
        public ActionResult EditProposal(ProposedMeetingModels model) {
            if(!ModelState.IsValid) {
                return View("EditProposal");
            }
            model.HostId = User.Identity.GetUserId();
            proposedMeetingRepository.Edit(model);
            proposedMeetingRepository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult ProposalInvitePeople(int Id) {
            List<ProfileModels> allProfiles = profileRepository.GetAllProfilesExceptCurrent(User.Identity.GetUserId());
            List<ProposalInviteModels> inviteModels = proposalInviteRepository.GetAll();
            ProposalInviteViewModels model = new ProposalInviteViewModels {
                ProposalId = Id,
                Profiles = allProfiles.OrderBy(profile => profile.FirstName).ToList(),
                Invites = inviteModels
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddProposalInvite(InviteViewModels vessel) {
            if(!ModelState.IsValid) {
                return Json(new { result = false });
            }
            string currentUserId = User.Identity.GetUserId();
            // Get all proposed times (for each of the up to three proposed times)
            List<ProposalInviteModels> proposalInvites = proposalInviteRepository.GetAllInvitesByProposalIdAndProfileId(vessel.ProposalId, currentUserId);
            // Foreach invite (up to three), fill.
            foreach(ProposalInviteModels invite in proposalInvites) {
                ProposalInviteModels proposalInvite = new ProposalInviteModels {
                    ProposalId = vessel.ProposalId,
                    ProfileId = vessel.ProfileId,
                    NotificationDateTime = DateTime.Now,
                    ProposedDateTime = invite.ProposedDateTime,
                    Accepted = false
                };
                proposalInviteRepository.Add(proposalInvite);
            }
            proposalInviteRepository.Save();
            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult RemoveProposalInvite(int proposalId, string profileId) {
            List<ProposalInviteModels> proposalInvites = proposalInviteRepository.GetAllInvitesByProposalIdAndProfileId(proposalId, profileId);
            foreach(ProposalInviteModels invite in proposalInvites) {
                proposalInviteRepository.Remove(invite.Id);
            }
            proposalInviteRepository.Save();
            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult AcceptProposalInvite(int id) {
            string currentUserId = User.Identity.GetUserId();
            ProposalInviteModels proposalInvite = proposalInviteRepository.Get(id);
            proposalInvite.Accepted = true;
            proposalInviteRepository.Edit(proposalInvite);
            proposalInviteRepository.Save();
            List<ProposalInviteModels> RemoveExcess = proposalInviteRepository.GetAllInvitesByProposalIdAndProfileId(proposalInvite.ProposalId, currentUserId);
            foreach(ProposalInviteModels invite in RemoveExcess) {
                if(!invite.Accepted) {
                    proposalInviteRepository.Remove(invite.Id);
                }
            }
            proposalInviteRepository.Save();
            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult DeclineProposalInvite(int id) {
            proposalInviteRepository.Remove(id);
            proposalInviteRepository.Save();
            return Json(new { result = true });
        }

        [HttpPost]
        public PartialViewResult GetParticipantsContent(int id) {
            List<ProposalInviteModels> proposalInvites = proposalInviteRepository.GetAllInvitesByProposalId(id).Where(invite => invite.Accepted).ToList();
            return PartialView("_ParticipantsList", proposalInvites);
        }
    }
}