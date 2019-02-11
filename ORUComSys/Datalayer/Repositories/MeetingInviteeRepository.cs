using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class MeetingInviteeRepository : Repository<MeetingInviteeModels, int> {
        public MeetingInviteeRepository(ApplicationDbContext context) : base(context) { }

        public List<MeetingInviteeModels> GetAllMeetingInvitesForUserId(string userId) {
            return items.Where((x) => x.ProfileId.Equals(userId)).ToList();
        }

        public MeetingInviteeModels GetMeetingInviteByUserIdAndMeetingId(string userId, int meetingId) {
            return items.First((i) => i.ProfileId.Equals(userId) && i.MeetingId.Equals(meetingId));
        }
    }
}