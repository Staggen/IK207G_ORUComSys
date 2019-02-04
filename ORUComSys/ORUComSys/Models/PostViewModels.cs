using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class PostViewModels {
        public ProfileModels CurrentUser { get; set; }
        public List<PostModels> PostList { get; set; }
    }
}