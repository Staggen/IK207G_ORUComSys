using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class ProposedMeetingModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Host")]
        public string HostId { get; set; }
        public virtual ProfileModels Host { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public MeetingType Type { get; set; }

        public string Description { get; set; }
    }
}