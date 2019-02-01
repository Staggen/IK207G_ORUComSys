using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ProfileRepository : Repository<ProfileModels, string> {
        public ProfileRepository(ApplicationDbContext context) : base(context) { }

        public List<ProfileModels> GetAllProfilesExceptCurrent(string userId) {
            return items.Where((p) => !p.Id.Equals(userId)).ToList();
        }

        public bool IfProfileExists(string userId) {
            List<ProfileModels> profile = items.Where((p) => p.Id.Equals(userId)).ToList(); // Should only ever be 0 or 1
            if (profile.Count == 1) return true;
            else return false;
        }
    }
}