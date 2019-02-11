using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ProposalInviteeRepository : Repository<ProposalInviteeModels, int> {
        public ProposalInviteeRepository(ApplicationDbContext context) : base(context) { }

        public List<ProposalInviteeModels> GetAllProposalInvitesForUserId(string userId) {
            return items.Where((x) => x.UserId.Equals(userId)).ToList();
        }
    }
}
