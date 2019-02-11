using Datalayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); // Enable cascade delete when you remove something that requires it.
            base.OnModelCreating(modelBuilder);
        }

        // Start of DbSet(s)

        // Users
        public DbSet<ProfileModels> Profiles { get; set; }
        // Posts
        public DbSet<PostModels> Posts { get; set; }
        public DbSet<CategoryModels> Categories { get; set; }
        // Meetings
        public DbSet<MeetingModels> Meetings { get; set; }
        public DbSet<MeetingInviteeModels> MeetingInvitees { get; set; }
        // Meeting Proposals (not done)
        //public DbSet<MeetingProposalModels> Proposals { get; set; }
        //public DbSet<MeetingInviteeModels> ProposalInvitees { get; set; }
        // Reactions
        public DbSet<ReactionModels> Reactions { get; set; }

        // End of DbSet(s)
    }
}