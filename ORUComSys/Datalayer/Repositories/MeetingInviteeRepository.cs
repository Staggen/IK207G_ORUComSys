using Datalayer.Models;

namespace Datalayer.Repositories {
    public class MeetingInviteeRepository : Repository<MeetingInviteeModels, int> {
        public MeetingInviteeRepository(ApplicationDbContext context) : base(context) { }

    }
}