using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class EmailViewModels {
        public ProfileModels Sender { get; set; }
        public ProfileModels Recipient { get; set; }
        public MeetingModels Meeting { get; set; }
        public PostModels Post { get; set; }
        public List<AttachmentModels> Attachments { get; set; }
        public string CategoryName { get; set; }
    }
}