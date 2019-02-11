using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class ProposalInviteeModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Proposal")]
        public int ProposalId { get; set; }
        public virtual MeetingProposalModels Proposal { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ProfileModels User { get; set; }

        public bool ProposalAccepted { get; set; }
    }
}