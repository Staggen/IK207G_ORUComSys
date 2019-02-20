using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ProposedMeetingRepository : Repository<ProposedMeetingModels, int> {
        public ProposedMeetingRepository(ApplicationDbContext context) : base(context) { }

        public List<ProposedMeetingModels> GetAllProposedMeetingsByHostId(string profileId) {
            return items.Where(proposedMeeting => proposedMeeting.HostId.Equals(profileId)).ToList();
        }

        public List<ProposedMeetingModels> GetProposedMeetingsByProposalIds(List<int> proposalIds) {
            return items.Where(proposal => proposalIds.Any(id => id.Equals(proposal.Id))).ToList();
        }

        public ProposedMeetingModels GetProposedMeetingByProposalId(int proposalId) {
            return items.Single(proposal => proposal.Id.Equals(proposalId));
        }
    }
}