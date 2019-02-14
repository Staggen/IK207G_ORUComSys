using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class MeetingInviteRepository : Repository<MeetingInviteModels, int> {
        public MeetingInviteRepository(ApplicationDbContext context) : base(context) { }

        public List<MeetingInviteModels> GetAllMeetingInvitesForProfileId(string profileId) {
            return items.Where((i) => i.ProfileId.Equals(profileId)).ToList();
        }

        public MeetingInviteModels GetMeetingInvite(string profileId, int meetingId) {
            return items.First((i) => i.ProfileId.Equals(profileId) && i.MeetingId.Equals(meetingId));
        }
    }
}