using Datalayer.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ORUComSys.Models {
    public class ProposedMeetingVesselModel {
        [Required]
        public string Location { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Display(Name = "First Time (YYYY-MM-DD HH:MM)")]
        [DataType(DataType.DateTime)]
        public DateTime FirstTime { get; set; }

        [Display(Name = "Second Time (YYYY-MM-DD HH:MM)")]
        [DataType(DataType.DateTime)]
        public DateTime? SecondTime { get; set; }

        [Display(Name = "Third Time (YYYY-MM-DD HH:MM)")]
        [DataType(DataType.DateTime)]
        public DateTime? ThirdTime { get; set; }

        [Required]
        public MeetingType Type { get; set; }
    }
}