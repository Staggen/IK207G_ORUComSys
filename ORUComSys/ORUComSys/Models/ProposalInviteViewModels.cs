using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class ProposalInviteViewModels {
        public int ProposalId { get; set; }
        public List<ProfileModels> Profiles { get; set; }
        public List<ProposalInviteModels> Invites { get; set; }
    }
}