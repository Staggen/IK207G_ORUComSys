using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class MeetingInviteeModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Meeting")]
        public int MeetingId { get; set; }
        public virtual MeetingModels Meeting { get; set; }

        [Required]
        [ForeignKey("Profile")]
        public string ProfileId { get; set; }
        public virtual ProfileModels Profile { get; set; }

        public bool MeetingAccepted { get; set; }
    }
}