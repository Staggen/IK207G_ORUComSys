using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ORUComSys.Controllers
{
    public class MeetingController : Controller
    {
        private MeetingProposalRepository MeetingProposalRepository;
        private MeetingInviteeRepository MeetingInviteeRepository;
        private DeterminedMeetingRepository DeterminedMeetingRepository;
        private UserRepository UserRepository;
        private ProfileRepository ProfileRepository;
        public MeetingController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            MeetingProposalRepository = new MeetingProposalRepository(context);
            MeetingInviteeRepository = new MeetingInviteeRepository(context);
            DeterminedMeetingRepository = new DeterminedMeetingRepository(context);
            UserRepository = new UserRepository(context);
            ProfileRepository = new ProfileRepository(context);
        }

        //metoden tar en MeetingProposal
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(MeetingProposalModels meeting)
        {
            var proposal = new MeetingProposalModels
            {
                Description = meeting.Description,
                StartTimes = meeting.StartTimes,
                Id = meeting.Id,
                Location = meeting.Location,
                Type = meeting.Type,
                Host = UserRepository.Get(User.Identity.GetUserId())
            };
            MeetingProposalRepository.Add(proposal);
            MeetingProposalRepository.Save();
            return RedirectToAction("Invited", "Meeting", new {id=proposal.Id});
        }

        public ActionResult Invited(int id)
        {
            ViewBag.Id = id;
            var ListOfUsers = ProfileRepository.GetAll();
            return View(ListOfUsers);
        }

        [HttpPost]
        public void Invited(int Id, string userId)
        {
            var invite = new MeetingInviteeModels
            {
                Proposal = Id,
                User = UserRepository.Get(userId),
                Accepted = false
            };
            MeetingInviteeRepository.Add(invite);
            MeetingInviteeRepository.Save();
        }


        public ActionResult Determind()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Determind(MeetingProposalModels model)
        {
            var ListOfInvited = new List<MeetingInviteeModels>();
            foreach (var item in MeetingInviteeRepository.GetAll())
            {
                if (model.Id == item.Id && item.Accepted == true)
                {
                    ListOfInvited.Add(item);
                }
            }
            var meeting = new DeterminedMeetings
            {
                Id = model.Id,
                Amount = ListOfInvited.Count,
                Date = model.StartTimes,
                Title = model.Description
            };
            DeterminedMeetingRepository.Add(meeting);
            DeterminedMeetingRepository.Save();
            return View();
        }
    }
}