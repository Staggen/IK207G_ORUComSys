using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class FollowingCategoryModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual CategoryModels Category { get; set; }
    }
}