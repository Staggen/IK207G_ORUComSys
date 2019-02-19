using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ProposalInviteRepository : Repository<ProposalInviteModels, int> {
        public ProposalInviteRepository(ApplicationDbContext context) : base(context) { }

        public List<ProposalInviteModels> GetAllProposalInvitesForUserId(string profileId) {
            return items.Where(proposalInvite => proposalInvite.ProfileId.Equals(profileId)).ToList();
        }
    }
}
