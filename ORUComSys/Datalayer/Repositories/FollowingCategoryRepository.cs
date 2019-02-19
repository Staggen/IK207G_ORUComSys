using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class FollowingCategoryRepository : Repository<FollowingCategoryModels, int> {
        public FollowingCategoryRepository(ApplicationDbContext context) : base(context) { }

        public List<FollowingCategoryModels> GetAllFollowedCategoriesByUserId(string profileId) {
            return items.Where(followedCategory => followedCategory.ProfileId.Equals(profileId)).ToList();
        }

        public List<string> GetAllUserIdsByFollowedCategoryId(int categoryId) {
            return items.Where(followedCategory => followedCategory.CategoryId.Equals(categoryId)).Select(followedCategory => followedCategory.ProfileId).ToList();
        }
    }
}