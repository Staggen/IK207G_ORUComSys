using Datalayer.Models;

namespace Datalayer.Repositories {
    public class MeetingProposalRepository : Repository<MeetingProposalModels, int> {
        public MeetingProposalRepository(ApplicationDbContext context) : base(context) { }

    }
}