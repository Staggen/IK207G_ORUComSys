using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class MeetingInviteModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Meeting")]
        public int MeetingId { get; set; }
        public virtual MeetingModels Meeting { get; set; }

        [Required]
        [ForeignKey("Profile")]
        public string ProfileId { get; set; }
        public virtual ProfileModels Profile { get; set; }

        [Required]
        public DateTime InviteDateTime { get; set; }

        [Required]
        public bool Accepted { get; set; } = false;
    }
}