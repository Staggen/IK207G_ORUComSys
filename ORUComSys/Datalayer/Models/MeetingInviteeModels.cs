using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class MeetingInviteeModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Meeting")]
        public int Proposal { get; set; }
        public virtual MeetingProposalModels Meeting { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public bool Accepted { get; set; }
    }
}