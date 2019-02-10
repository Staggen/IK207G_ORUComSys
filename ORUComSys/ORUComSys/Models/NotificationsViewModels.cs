using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class NotificationsViewModels {
        public List<PostModels> Posts { get; set; }
        public List<MeetingInviteeModels> Invites { get; set; }
        public List<MeetingProposalModels> Proposals { get; set; }
    }
}