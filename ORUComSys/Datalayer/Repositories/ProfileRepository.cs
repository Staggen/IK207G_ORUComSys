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
            return items.Any((p) => p.Id.Equals(userId));
        }
    }
}