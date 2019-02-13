using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class MeetingRepository : Repository<MeetingModels, int> {
        public MeetingRepository(ApplicationDbContext context) : base(context) { }

        public List<MeetingModels> GetAllMeetingsByCreatorId(string userId) {
            return items.Where((m) => m.CreatorId.Equals(userId)).ToList();
        }

        public List<MeetingModels> GetListOfMeetingsByListOfMeetingIds(List<int> meetingIds) {
            return items.Where((m) => meetingIds.Any((i) => i.Equals(m.Id))).ToList();
        }
    }
}