using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ReactionRepository : Repository<ReactionModels, int> {
        public ReactionRepository(ApplicationDbContext context) : base(context) { }

        public List<ReactionModels> GetAllReactionsByPostId(int postId) {
            return items.Where((r) => r.PostId.Equals(postId)).OrderByDescending((p) => p.Reaction).ToList();
        }

        public bool GetReactionByPostAndUserIdBool(int postId, string userId) {
            return items.Any((r) => r.PostId.Equals(postId) && r.UserId.Equals(userId));
        }

        public ReactionModels GetReactionByPostAndUserId(int postId, string userId) {
            return items.First((r) => r.PostId.Equals(postId) && r.UserId.Equals(userId)); // PASSES BOOL CHECK; DOESN'T FIND ELEMENT. FIX TOMORROW #SLACK
        }
    }
}