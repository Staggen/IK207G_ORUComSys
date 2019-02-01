using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class MeetingInviteeModels : IIdentifiable<int> {
        [Key]
        [ForeignKey("Proposal")]
        public int Id { get; set; }
        public virtual MeetingProposalModels Proposal { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}