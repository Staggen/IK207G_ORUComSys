using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class MeetingRepository : Repository<MeetingModels, int> {
        public MeetingRepository(ApplicationDbContext context) : base(context) { }

        public List<MeetingModels> GetAllMeetingsByCreatorId(string profileId) {
            return items.Where(meeting => meeting.CreatorId.Equals(profileId)).ToList();
        }

        public List<MeetingModels> GetListOfMeetingsByMeetingIds(List<int> meetingIds) {
            return items.Where(meeting => meetingIds.Any((i) => i.Equals(meeting.Id))).ToList();
        }
    }
}