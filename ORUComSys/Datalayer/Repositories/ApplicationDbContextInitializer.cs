using Datalayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.IO;

namespace Datalayer.Repositories {
    public class ApplicationDbContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext> { // Re-create database with example data every time you boot the project.
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
                Email = "elias@orucomsys.com",
                UserName = "elias@orucomsys.com"
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
                Email = "oskar@orucomsys.com",
                UserName = "oskar@orucomsys.com"
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
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels darioP = new ProfileModels {
                Id = darioU.Id,
                FirstName = "Dario",
                LastName = "Borojevic",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels eliasP = new ProfileModels {
                Id = eliasU.Id,
                FirstName = "Elias",
                LastName = "Stagg",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels moazP = new ProfileModels {
                Id = moazU.Id,
                FirstName = "Moaz",
                LastName = "Bahtiti",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels nicoP = new ProfileModels {
                Id = nicoU.Id,
                FirstName = "Nicolas",
                LastName = "Björkefors",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels oskarP = new ProfileModels {
                Id = oskarU.Id,
                FirstName = "Oskar",
                LastName = "Olofsson",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels patrikP = new ProfileModels {
                Id = patrikU.Id,
                FirstName = "Patrik",
                LastName = "Zetterblom",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels pernillaP = new ProfileModels {
                Id = pernillaU.Id,
                FirstName = "Pernilla",
                LastName = "Gerdin",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            ProfileModels salehP = new ProfileModels {
                Id = salehU.Id,
                FirstName = "Saleh",
                LastName = "Hassan",
                IsActivated = true,
                ProfileImage = SetInitializerProfilePicture("/Content/Images/defaultAvatar.png")
            };

            context.Profiles.AddRange(new[] { albinP, darioP, eliasP, moazP, nicoP, oskarP, patrikP, pernillaP, salehP });
            context.SaveChanges();
        }
    }
}