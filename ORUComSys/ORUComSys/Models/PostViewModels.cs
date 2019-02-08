using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ORUComSys.Models {
    public class PostViewModels {
        public int Id { get; set; }

        public ProfileModels PostFrom { get; set; }

        public ForumType Forum { get; set; }

        public CategoryModels Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime PostDateTime { get; set; }

        public HttpPostedFileBase AttachedFile { get; set; }

        public ProfileModels CurrentUser { get; set; }
    }

    public class PostViewModelsForUsers {
        public List<PostViewModels> PostList { get; set; }
        public ProfileModels CurrentUser { get; set; }
    }
}