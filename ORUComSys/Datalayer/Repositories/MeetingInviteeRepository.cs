using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class MeetingInviteeRepository : Repository<MeetingInviteeModels, int> {
        public MeetingInviteeRepository(ApplicationDbContext context) : base(context) { }

        public List<MeetingInviteeModels> GetAllMeetingProposalsForUserId(string userId) {
            return items.Where((x) => x.UserId.Equals(userId)).ToList();
        }
    }
}