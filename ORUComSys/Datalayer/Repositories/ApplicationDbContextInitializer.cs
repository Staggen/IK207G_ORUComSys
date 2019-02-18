using Datalayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.IO;

namespace Datalayer.Repositories {
    public class ApplicationDbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> { // Re-create database with example data every time you boot the project.
        protected override void Seed(ApplicationDbContext context) {
            base.Seed(context);
            SeedUsers(context);
        }

        public static byte[] SetInitializerProfilePicture(string endPath) {
            // Setting default avatar for all profiles
            string path = AppDomain.CurrentDomain.BaseDirectory + endPath;
            FileStream file = new FileStream(path, FileMode.Open);
            byte[] avatar = null;
            using (var binary = new BinaryReader(file)) {
                avatar = binary.ReadBytes((int)file.Length);
            }
            return avatar;
        }

        public static void SeedUsers(ApplicationDbContext context) {
            // Seed users into database
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);

            ApplicationUser albinU = new ApplicationUser {
                Email = "albin@orucomsys.com",
                UserName = "albin@orucomsys.com"
            };
            manager.Create(albinU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser darioU = new ApplicationUser {
                Email = "dario@orucomsys.com",
                UserName = "dario@orucomsys.com"
            };
            manager.Create(darioU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser eliasU = new ApplicationUser {
                Email = "elias.stagg@gmail.com",
                UserName = "elias.stagg@gmail.com"
            };
            manager.Create(eliasU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser moazU = new ApplicationUser {
                Email = "moaz@orucomsys.com",
                UserName = "moaz@orucomsys.com"
            };
            manager.Create(moazU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser nicoU = new ApplicationUser {
                Email = "nico@orucomsys.com",
                UserName = "nico@orucomsys.com"
            };
            manager.Create(nicoU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser oskarU = new ApplicationUser {
                Email = "orre.b3000@gmail.com",
                UserName = "orre.b3000@gmail.com"
            };
            manager.Create(oskarU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser patrikU = new ApplicationUser {
                Email = "patrik@orucomsys.com",
                UserName = "patrik@orucomsys.com"
            };
            manager.Create(patrikU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser pernillaU = new ApplicationUser {
                Email = "pernilla@orucomsys.com",
                UserName = "pernilla@orucomsys.com"
            };
            manager.Create(pernillaU, "password"); // manager.Create(ApplicationUser user, string password);

            ApplicationUser salehU = new ApplicationUser {
                Email = "saleh@orucomsys.com",
                UserName = "saleh@orucomsys.com"
            };
            manager.Create(salehU, "password"); // manager.Create(ApplicationUser user, string password);

            // Create more example data as you create more DbSets as the database flushes and resets every time you boot the project. (Current initializer setting: DropCreateDatabaseAlways<{context}>)

            ProfileModels albinP = new ProfileModels {
                Id = albinU.Id,
                FirstName = "Albin",
                LastName = "Salmonson",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels darioP = new ProfileModels {
                Id = darioU.Id,
                FirstName = "Dario",
                LastName = "Borojevic",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels eliasP = new ProfileModels {
                Id = eliasU.Id,
                FirstName = "Elias",
                LastName = "Stagg",
                IsActivated = true,
                IsAdmin = true,
                PhoneNumber = 12345678,
                Title = "Bugs'R'Us Manager",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels moazP = new ProfileModels {
                Id = moazU.Id,
                FirstName = "Moaz",
                LastName = "Bahtiti",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels nicoP = new ProfileModels {
                Id = nicoU.Id,
                FirstName = "Nicolas",
                LastName = "Björkefors",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels oskarP = new ProfileModels {
                Id = oskarU.Id,
                FirstName = "Oskar",
                LastName = "Olofsson",
                IsAdmin = true,
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels patrikP = new ProfileModels {
                Id = patrikU.Id,
                FirstName = "Patrik",
                LastName = "Zetterblom",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels pernillaP = new ProfileModels {
                Id = pernillaU.Id,
                FirstName = "Pernilla",
                LastName = "Gerdin",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            ProfileModels salehP = new ProfileModels {
                Id = salehU.Id,
                FirstName = "Saleh",
                LastName = "Hassan",
                IsActivated = true,
                PhoneNumber = 12345678,
                Title = "Lord of the Universe",
                Description = "Why do we get to set our own titles?",
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png"),
                LastLogin = DateTime.Now
            };

            // Define Categories
            CategoryModels cat1 = new CategoryModels {
                Name = "Notes",
                Category = CategoryType.Notes
            };

            CategoryModels cat2 = new CategoryModels {
                Name = "Economy",
                Category = CategoryType.Economy
            };

            CategoryModels cat3 = new CategoryModels {
                Name = "Event",
                Category = CategoryType.Event
            };

            CategoryModels cat4 = new CategoryModels {
                Name = "Security",
                Category = CategoryType.Security
            };

            CategoryModels cat5 = new CategoryModels {
                Name = "Other",
                Category = CategoryType.Other
            };

            // Define Meethings
            MeetingModels meeting1 = new MeetingModels {
                CreatorId = eliasU.Id,
                Title = "Sprintplaneringsmöte #3",
                Description = "Detta är det tredje och sista sprintplaneringsmötet i denna kurs. Varför köpte ingen kakor?!",
                Location = "P254",
                MeetingDateTime = new DateTime(2019, 02, 12, 10, 00, 00),
                Type = MeetingType.Public
            };

            MeetingModels meeting2 = new MeetingModels {
                CreatorId = eliasU.Id,
                Title = "Another One",
                Description = "- By DJ Khaled",
                Location = "Where the Sun Don't Shine",
                MeetingDateTime = new DateTime(2039, 12, 03, 14, 32, 00),
                Type = MeetingType.Public
            };

            context.Meetings.AddRange(new[] { meeting1, meeting2 });
            context.Categories.AddRange(new[] { cat1, cat2, cat3, cat4, cat5 });
            context.Profiles.AddRange(new[] { albinP, darioP, eliasP, moazP, nicoP, oskarP, patrikP, pernillaP, salehP });
            context.SaveChanges();

            // Define Meeting Invites
            MeetingInviteModels invite1 = new MeetingInviteModels {
                ProfileId = eliasU.Id,
                MeetingId = meeting1.Id,
                InviteDateTime = new DateTime(2019, 02, 12, 13, 37, 00),
                Accepted = true
            };
            MeetingInviteModels invite2 = new MeetingInviteModels {
                ProfileId = eliasU.Id,
                MeetingId = meeting2.Id,
                InviteDateTime = new DateTime(2019, 02, 12, 13, 38, 00),
                Accepted = true
            };
            MeetingInviteModels invite3 = new MeetingInviteModels {
                ProfileId = oskarU.Id,
                MeetingId = meeting1.Id,
                InviteDateTime = new DateTime(2019, 02, 12, 13, 39, 00),
                Accepted = false
            };
            MeetingInviteModels invite4 = new MeetingInviteModels {
                ProfileId = patrikU.Id,
                MeetingId = meeting1.Id,
                InviteDateTime = new DateTime(2019, 02, 12, 13, 40, 00),
                Accepted = false
            };
            MeetingInviteModels invite5 = new MeetingInviteModels {
                ProfileId = oskarU.Id,
                MeetingId = meeting2.Id,
                InviteDateTime = new DateTime(2019, 02, 12, 13, 41, 00),
                Accepted = false
            };

            // Define Posts
            PostModels post1 = new PostModels {
                PostFromId = nicoU.Id,
                Forum = ForumType.Formal,
                Content = "Please see the attached file for notes from the annual cheese making meeting!",
                CategoryId = 1,
                PostDateTime = new DateTime(2019, 01, 13, 23, 45, 02)
            };
            PostModels post2 = new PostModels {
                PostFromId = eliasU.Id,
                Forum = ForumType.Formal,
                Content = "Look ma, I can react to things!",
                CategoryId = 2,
                PostDateTime = new DateTime(2019, 02, 10, 18, 45, 00)
            };

            context.MeetingInvites.AddRange(new[] { invite1, invite2, invite3, invite4, invite5 });
            context.Posts.AddRange(new[] { post1, post2 });
            context.SaveChanges();

            // Define Reactions

            ReactionModels reaction1 = new ReactionModels {
                PostId = post1.Id,
                ProfileId = eliasU.Id,
                Reaction = ReactionType.Like
            };
            ReactionModels reaction2 = new ReactionModels {
                PostId = post1.Id,
                ProfileId = oskarU.Id,
                Reaction = ReactionType.Hate
            };
            ReactionModels reaction3 = new ReactionModels {
                PostId = post2.Id,
                ProfileId = eliasU.Id,
                Reaction = ReactionType.XD
            };
            ReactionModels reaction4 = new ReactionModels {
                PostId = post2.Id,
                ProfileId = patrikU.Id,
                Reaction = ReactionType.Hate
            };

            context.Reactions.AddRange(new[] { reaction1, reaction2, reaction3, reaction4 });
            context.SaveChanges();
        }
    }
}