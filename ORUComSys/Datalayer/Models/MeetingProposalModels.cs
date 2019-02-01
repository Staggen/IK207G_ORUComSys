using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class MeetingProposalModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Host")]
        public string HostId { get; set; }
        public virtual ApplicationUser Host { get; set; }
        [Required]
        public string Location { get; set; }
        [Display(Name = "Start time")]
        [DataType(DataType.Date)]
        public DateTime StartTimes { get; set; }
        [Required]
        public MeetingType Type { get; set; }
        public string Description { get; set; }
    }

    public enum MeetingType {
        Public,
        Private,
        Secret
    }
}