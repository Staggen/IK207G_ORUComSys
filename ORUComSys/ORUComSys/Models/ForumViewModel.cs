using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ORUComSys.Models {
    // Model for FormalForum and InformalForum views
    public class ForumViewModel {
        [Required]
        public CategoryType Category { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public List<CategoryModels> Categories { get; set; }

        public PostsViewModel PartialViewModel { get; set; }
        
    }
    // Model for _ForumPosts partial view
    public class PostsViewModel {
        public ProfileModels CurrentUser { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
    // Vessel to attach other models to
    public class PostViewModel {
        public int Id { get; set; }
        public ProfileModels PostFrom { get; set; }
        public ForumType Forum { get; set; }
        [Required]
        public CategoryType Category { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime PostDateTime { get; set; }
        public ProfileModels CurrentPoster { get; set; }
        // Other models attached to posts
        public CommentsViewModel Comments { get; set; }
        public List<AttachmentModels> Attachments { get; set; }
        public List<ReactionModels> Reactions { get; set; }
        public List<CategoryModels> Categories { get; set; }
    }
    public class CommentsViewModel {
        public ProfileModels CurrentCommenter { get; set; }
        public List<CommentModels> Comments { get; set; }
    }
}