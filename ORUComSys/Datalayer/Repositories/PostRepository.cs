using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class PostRepository : Repository<PostModels, int> {
        public PostRepository(ApplicationDbContext context) : base(context) { }

        public List<PostModels> GetAllPostsForUserById(string userId) {
            return items.Where((p) => p.PostFromId.Equals(userId)).OrderByDescending((p) => p.PostDateTime).ToList();
        }

        public List<PostModels> GetAllPostsByForumType(ForumType type) {
            return items.Where((p) => p.Forum == type).OrderByDescending((p) => p.PostDateTime).ToList();
        }

        public PostModels GetLastPostCreatedByUserId(string userId) {
            return items.Where((x) => x.PostFromId.Equals(userId)).OrderByDescending((p) => p.PostDateTime).First();
        }
        
        public List<PostModels> GetAllPostsInCategorySinceLastUserLoginByUserId(List<FollowingCategoryModels> followedCategories, DateTime lastLogin) {
            // ERROR - ShitInProgress.
            return items.Where((p) => followedCategories.Any((c) => c.CategoryId.Equals(p.CategoryId)) && p.PostDateTime > lastLogin).ToList();
        }
    }
}