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
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); // Enable cascade delete when you remove something that requires it.
            base.OnModelCreating(modelBuilder);
        }

        // Start of DbSet(s)

        // public DbSet<{Model}> {Name} { get; set; }
        public DbSet<CategoryModels> Categories { get; set; }
        public DbSet<MeetingInviteeModels> Invitees { get; set; }
        //public DbSet<MeetingModels> Meetings { get; set; } // Enable when we have meeting models
        public DbSet<MeetingProposalModels> Proposals { get; set; }
        public DbSet<PostModels> Posts { get; set; }
        public DbSet<ProfileModels> Profiles { get; set; }
        public DbSet<DeterminedMeetings> DeterminedMeetings { get; set; }
        // End of DbSet(s)
    }
}