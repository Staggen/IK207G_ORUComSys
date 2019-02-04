using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORUComSys.Models {
    public class PostViewModels {
        public ProfileModels CurrentUser { get; set; }
        public List<PostModels> PostList { get; set; }
    }
}