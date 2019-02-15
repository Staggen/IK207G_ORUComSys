using System.ComponentModel.DataAnnotations;

namespace ORUComSys.Models {
    public class InviteViewModels {
        public int MeetingId { get; set; }
        public int ProposalId { get; set; }

        [Required]
        public string ProfileId { get; set; }
    }
}