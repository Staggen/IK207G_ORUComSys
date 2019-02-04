using Datalayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models
{
   public class DeterminedMeetings : IIdentifiable<int>
    {
        [Key]
        [ForeignKey("Determined")]
        public int Id { get; set; }
        public virtual MeetingProposalModels Determined { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }

    }
}
