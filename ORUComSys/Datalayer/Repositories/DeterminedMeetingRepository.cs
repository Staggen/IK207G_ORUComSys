using Datalayer.Models;

namespace Datalayer.Repositories
{
    public class DeterminedMeetingRepository : Repository<DeterminedMeetings, int>
    {
        public DeterminedMeetingRepository(ApplicationDbContext context) : base(context) { }
    }
}

