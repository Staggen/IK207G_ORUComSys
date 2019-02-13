using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class FollowingUserModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Profile")]
        public string ProfileId { get; set; }
        public virtual ProfileModels Profile { get; set; }

        [ForeignKey("FollowedUser")]
        public string FollowedUserId { get; set; }
        public virtual ApplicationUser FollowedUser { get; set; }
    }
}