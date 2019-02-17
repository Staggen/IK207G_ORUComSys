using Datalayer.Models;

namespace ORUComSys.Models {
    public class EmailViewModels {
        public ProfileModels Sender { get; set; }
        public ProfileModels Recipient { get; set; }
        public MeetingModels Meeting { get; set; }
    }
}