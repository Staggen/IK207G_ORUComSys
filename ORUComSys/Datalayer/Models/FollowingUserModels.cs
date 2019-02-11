using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class FollowingUserModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ProfileModels User { get; set; }

        [ForeignKey("FollowedUser")]
        public string FollowedUserId { get; set; }
        public virtual ApplicationUser FollowedUser { get; set; }
    }
}