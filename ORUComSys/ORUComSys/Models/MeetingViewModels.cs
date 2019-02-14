using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class MeetingViewModels {
        public string ProfileId { get; set; }
        public List<MeetingModels> MyMeetings { get; set; }
        public List<MeetingModels> MyCreatedMeetings { get; set; }
        public List<MeetingInviteModels> Invites { get; set; }
    }
}