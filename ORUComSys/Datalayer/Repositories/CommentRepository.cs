using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class CommentRepository : Repository<CommentModels, int> {
        public CommentRepository(ApplicationDbContext context) : base(context) { }

        public List<CommentModels> GetAllCommentsByPostId(int postId) {
            return items.Where(comment => comment.PostId.Equals(postId)).OrderBy(comment => comment.CommentDateTime).ToList();
        }
    }
}