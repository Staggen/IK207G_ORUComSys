using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class MeetingModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }
        public virtual ProfileModels Creator { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Time and Date (YYYY-MM-DD HH:MM)")]
        public DateTime MeetingDateTime { get; set; }

        [Required]
        public MeetingType Type { get; set; }
    }
    public enum MeetingType {
        Public,
        Private,
        Secret
    }
}