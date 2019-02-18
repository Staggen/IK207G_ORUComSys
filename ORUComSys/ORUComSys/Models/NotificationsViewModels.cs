using Datalayer.Models;
using System.Collections.Generic;

namespace ORUComSys.Models {
    public class NotificationsViewModels {
        public List<MeetingInviteModels> Invites { get; set; }
        public List<PostModels> Posts { get; set; }
        public List<ProfileModels> PostFrom { get; set; }
    }
}