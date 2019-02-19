using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ProfileRepository : Repository<ProfileModels, string> {
        public ProfileRepository(ApplicationDbContext context) : base(context) { }

        public List<ProfileModels> GetAllProfilesExceptCurrent(string profileId) {
            return items.Where(profile => !profile.Id.Equals(profileId)).ToList();
        }

        public bool IfProfileExists(string profileId) {
            return items.Any(profile => profile.Id.Equals(profileId));
        }
    }
}