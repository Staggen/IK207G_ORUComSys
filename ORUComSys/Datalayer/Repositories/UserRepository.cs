using Datalayer.Models;
using System.Linq;

namespace Datalayer.Repositories {
    public class UserRepository : Repository<ApplicationUser, string> {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public string GetEmailByUserId(string userId) {
            return items.Single((u) => u.Id.Equals(userId)).Email;
        }

        public string GetUserIdByEmail(string email) {
            return items.Single((u) => u.Email.Equals(email)).Id;
        }
    }
}