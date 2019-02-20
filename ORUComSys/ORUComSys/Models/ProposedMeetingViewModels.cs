using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class ProposedMeetingViewModels {
        public string ProfileId { get; set; }
        public List<ProposedMeetingModels> MyProposals { get; set; }
        public List<ProposedMeetingModels> MyCreatedProposals { get; set; }
        public List<ProposalInviteModels> ProposalInvites { get; set; }
        public List<ProposalInviteModels> MyProposalInvites { get; set; }
    }
}