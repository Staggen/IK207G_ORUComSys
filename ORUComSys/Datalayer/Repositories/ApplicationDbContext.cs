using Datalayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Diagnostics;

namespace Datalayer.Repositories {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
            Database.Log = s => Debug.WriteLine(s);
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        // Start of DbSet(s)

        // Profiles
        public DbSet<ProfileModels> Profiles { get; set; }
        // Posts
        public DbSet<PostModels> Posts { get; set; }
        public DbSet<AttachmentModels> Attachments { get; set; }
        public DbSet<CategoryModels> Categories { get; set; }
        public DbSet<CommentModels> Comments { get; set; }
        // Meetings
        public DbSet<MeetingModels> Meetings { get; set; }
        public DbSet<MeetingInviteModels> MeetingInvites { get; set; }
        // Meeting Proposals (not done)
        //public DbSet<MeetingProposalModels> Proposals { get; set; }
        //public DbSet<MeetingInviteeModels> ProposalInvitees { get; set; }
        // Reactions
        public DbSet<ReactionModels> Reactions { get; set; }
        // Following
        public DbSet<FollowingCategoryModels> FollowingCategories { get; set; }

        // End of DbSet(s)
    }
}