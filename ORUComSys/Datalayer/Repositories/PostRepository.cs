using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class PostRepository : Repository<PostModels, int> {
        public PostRepository(ApplicationDbContext context) : base(context) { }

        public List<PostModels> GetAllPostsForUserById(string userId) {
            return items.Where((p) => p.PostFromId.Equals(userId)).OrderByDescending((p) => p.PostDateTime).ToList();
        }
    }
}