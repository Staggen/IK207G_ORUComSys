using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class PostRepository : Repository<PostModels, int> {
        public PostRepository(ApplicationDbContext context) : base(context) { }

        // Does not seem to be used anywhere?
        //public List<PostModels> GetAllPostsFromProfileById(string profileId) {
        //    return items.Where(post => post.PostFromId.Equals(profileId)).OrderByDescending(post => post.PostDateTime).ToList();
        //}

        public List<PostModels> GetAllPostsByForumType(ForumType type) {
            return items.Where(post => post.Forum == type).OrderByDescending(post => post.PostDateTime).ToList();
        }

        public PostModels GetLastPostCreatedByProfileId(string profileId) {
            return items.Where(post => post.PostFromId.Equals(profileId)).OrderByDescending(post => post.PostDateTime).First();
        }
        
        public List<PostModels> GetAllPostsInFollowedCategoriesSinceLastLogout(List<int> followedCategoryIds, DateTime lastLogout, string profileId) {
            return items.Where(post => followedCategoryIds.Any(followedCategoryId => followedCategoryId.Equals(post.CategoryId)) && post.PostDateTime > lastLogout && !post.PostFromId.Equals(profileId)).ToList();
        }
    }
}