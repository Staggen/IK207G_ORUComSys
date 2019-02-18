using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class FollowingCategoryRepository : Repository<FollowingCategoryModels, int> {
        public FollowingCategoryRepository(ApplicationDbContext context) : base(context) { }

        public List<FollowingCategoryModels> GetAllFollowedCategoriesByUserId(string profileId) {
            return items.Where((fc) => fc.ProfileId.Equals(profileId)).ToList();
        }

        public List<string> GetAllUsersByCategory(int categoryId) {
            return items.Where((fc) => fc.CategoryId.Equals(categoryId)).Select((p) => p.ProfileId).ToList();
        }
    }
}