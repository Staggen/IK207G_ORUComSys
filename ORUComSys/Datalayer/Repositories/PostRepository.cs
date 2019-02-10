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

        public List<PostModels> GetAllPostsInCategorySinceLastUserLoginByUserId(string userId, List<FollowingCategoryModels> categories, DateTime lastLogin) {
            List<PostModels> postsInCategories = null;
            foreach(FollowingCategoryModels item in categories) {
                postsInCategories.AddRange(items.Where((p) => p.CategoryId.Equals(item.CategoryId)).ToList());
            }
            return postsInCategories.Where((p) => p.PostDateTime > lastLogin).ToList();
        }

        public List<PostModels> GetAllPostsFromFollowedUserByUserId(string userId, DateTime lastLogin) {
            return items.Where((p) => p.PostFromId.Equals(userId) && p.PostDateTime > lastLogin).ToList();
        }
    }
}