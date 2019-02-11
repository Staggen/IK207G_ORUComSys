using System.ComponentModel.DataAnnotations;

namespace ORUComSys.Models {
    public class InviteViewModel {
        public int MeetingId { get; set; }
        public int ProposalId { get; set; }

        [Required]
        public string ProfileId { get; set; }
    }
}