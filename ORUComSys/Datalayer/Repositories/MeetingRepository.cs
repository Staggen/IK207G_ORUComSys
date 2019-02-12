using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class MeetingRepository : Repository<MeetingModels, int> {
        public MeetingRepository(ApplicationDbContext context) : base(context) { }

        public List<MeetingModels> GetAllMeetingsByCreatorId(string userId) {
            return items.Where((m) => m.CreatorId.Equals(userId)).ToList();
        }

        public List<MeetingModels> GetMeetingsByMeetingIds(List<int> meetingIds) {
            List<MeetingModels> models = new List<MeetingModels>();
            foreach(int id in meetingIds) {
                models.Add(items.First((m) => m.Id.Equals(id)));
            }
            return models;
        }
    }
}