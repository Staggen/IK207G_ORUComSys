using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ProposalInviteRepository : Repository<ProposalInviteModels, int> {
        public ProposalInviteRepository(ApplicationDbContext context) : base(context) { }

        public List<ProposalInviteModels> GetAllInvitesByProfileId(string profileId) {
            return items.Where(proposalInvite => proposalInvite.ProfileId.Equals(profileId)).ToList();
        }

        public List<ProposalInviteModels> GetAllInvitesByProposalId(int proposalId) {
            return items.Where(proposalInvite => proposalInvite.ProposalId.Equals(proposalId)).OrderBy(proposalInvite => proposalInvite.NotificationDateTime).ToList();
        }

        public List<ProposalInviteModels> GetAllInvitesByProposalIdAndProfileId(int proposalId, string profileId) {
            return items.Where(proposalInvite => proposalInvite.ProfileId.Equals(profileId) && proposalInvite.ProposalId.Equals(proposalId)).ToList();
        }

        public List<ProposalInviteModels> GetAllInvitesByProposalIds(List<int> proposalIds) {
            return items.Where(proposalInvite => proposalIds.Any(id => id.Equals(proposalInvite.ProposalId))).ToList();
        }
    }
}
