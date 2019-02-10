using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class FollowingCategoryRepository : Repository<FollowingCategoryModels, int> {
        public FollowingCategoryRepository(ApplicationDbContext context) : base(context) { }

        public List<FollowingCategoryModels> GetAllFollowedCategoriesByUserId(string userId) {
            return items.Where((f) => f.UserId.Equals(userId)).ToList();
        }
    }
}