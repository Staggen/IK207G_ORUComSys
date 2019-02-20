using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ORUComSys.Models {
    public class PostViewModels {
        public int Id { get; set; }

        public ProfileModels PostFrom { get; set; }

        public ForumType Forum { get; set; }

        [Required]
        public CategoryType Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime PostDateTime { get; set; }

        public List<AttachmentModels> Attachments { get; set; }

        public ProfileModels CurrentUser { get; set; }

        public List<ReactionModels> Reactions { get; set; }

        public List<CategoryModels> Categories { get; set; }

        public List<CommentModels> Comments { get; set; }
    }

    public class PostViewModelsForUsers {
        public List<PostViewModels> Posts { get; set; }
        public ProfileModels CurrentUser { get; set; }
    }

    public class ForumViewModel {
        public PostViewModelsForUsers PartialViewModel { get; set; }

        public int Id { get; set; }

        public ProfileModels PostFrom { get; set; }

        public ForumType Forum { get; set; }

        [Required]
        public CategoryType Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime PostDateTime { get; set; }

        public List<AttachmentModels> Attachments { get; set; }

        public ProfileModels CurrentUser { get; set; }

        public List<ReactionModels> Reactions { get; set; }

        public List<CategoryModels> Categories { get; set; }

        public List<CommentModels> Comments { get; set; }
    }
}