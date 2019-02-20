using Datalayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace Datalayer.Repositories {
    public class UserRepository : Repository<ApplicationUser, string> {
        private UserManager<ApplicationUser> manager;

        public UserRepository(ApplicationDbContext context) : base(context) {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        public string GetEmailByUserId(string userId) {
            return items.Single(user => user.Id.Equals(userId)).Email;
        }

        public string GetUserIdByEmail(string email) {
            return items.Single(user => user.Email.Equals(email)).Id;
        }

        public void AddUserToProfiledRole(string userId) {
            manager.AddToRole(userId, "Profiled");
        }
    }
}