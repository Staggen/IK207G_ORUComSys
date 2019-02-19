using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class ReactionRepository : Repository<ReactionModels, int> {
        public ReactionRepository(ApplicationDbContext context) : base(context) { }

        public List<ReactionModels> GetAllReactionsByPostId(int postId) {
            return items.Where(reaction => reaction.PostId.Equals(postId)).OrderByDescending(reactionType => reactionType.Reaction).ToList();
        }

        public bool ReactionExists(int postId, string profileId) {
            return items.Any(reaction => reaction.PostId.Equals(postId) && reaction.ProfileId.Equals(profileId));
        }

        public ReactionModels GetReactionByPostAndProfileId(int postId, string profileId) {
            return items.First(reaction => reaction.PostId.Equals(postId) && reaction.ProfileId.Equals(profileId));
        }
    }
}