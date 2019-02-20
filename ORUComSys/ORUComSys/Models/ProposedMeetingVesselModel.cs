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

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FirstTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SecondTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ThirdTime { get; set; }

        [Required]
        [Display(Name = "Select Meeting Type")]
        public MeetingType Type { get; set; }
    }
}