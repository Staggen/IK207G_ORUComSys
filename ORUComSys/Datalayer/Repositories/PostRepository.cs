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

        // Uncomment the following when a user can subscribe to post categories and need this to recieve notifications about such.
        //public List<PostModels> GetAllPostsInCategorySinceLastUserLoginByUserId(List<FollowingCategoryModels> categories, DateTime lastLogin) {
        //    return items.Where((p) => categories.Any((c) => c.Id.Equals(p.CategoryId)) && p.PostDateTime > lastLogin).ToList();
        //}

        //public List<PostModels> GetAllPostsFromFollowedUserByUserId(string userId, DateTime lastLogin) {
        //    return items.Where((p) => p.PostFromId.Equals(userId) && p.PostDateTime > lastLogin).ToList();
        //}
    }
}