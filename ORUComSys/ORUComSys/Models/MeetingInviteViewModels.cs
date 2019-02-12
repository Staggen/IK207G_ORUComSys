using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class MeetingInviteViewModels {
        public int MeetingId { get; set; }
        public List<ProfileModels> Profiles { get; set; }
        public List<MeetingInviteeModels> Invites { get; set; }
    }
}