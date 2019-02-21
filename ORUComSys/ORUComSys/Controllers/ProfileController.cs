using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
    public class ProfileController : Controller {
        private ProfileRepository profileRepository;
        private UserRepository userRepository;

        public ProfileController() {
            ApplicationDbContext context = new ApplicationDbContext();
            profileRepository = new ProfileRepository(context);
            userRepository = new UserRepository(context);
        }

        [Authorize(Roles = "Profiled")]
        public ActionResult Index() {
            string currentUserId = User.Identity.GetUserId();
            object profileId = Request.RequestContext.RouteData.Values["id"];
            ProfileModels profile = null;
            if(!string.IsNullOrWhiteSpace((string)profileId)) {
                profile = profileRepository.Get((string)profileId);
                ViewBag.Banned = userRepository.Get((string)profileId).LockoutEnabled;
                ViewBag.ProfileId = (string)profileId;
                ViewBag.CurrentUserId = currentUserId;
                ViewBag.IsAdmin = profileRepository.Get(currentUserId).IsAdmin;
            } else {
                ViewBag.Banned = false;
                profile = profileRepository.Get(currentUserId);
                ViewBag.ProfileId = currentUserId;
                ViewBag.CurrentUserId = currentUserId;
                ViewBag.IsAdmin = profileRepository.Get(currentUserId).IsAdmin;
            }
            return View(profile);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ProfileImage")] ProfileModels profile) {
            // Excludes ProfileImage from controller call so the program does not crash.
            if(!ModelState.IsValid) { // If model is invalid
                return RedirectToAction("Create");
            }
            if(Request.Files["ProfileImage"].ContentLength >= 1) { // If a file was submitted
                HttpPostedFileBase profileImgFile = Request.Files["ProfileImage"];
                using(BinaryReader binary = new BinaryReader(profileImgFile.InputStream)) {
                    profile.ProfileImage = binary.ReadBytes(profileImgFile.ContentLength); // Set the profile image to the variable imageData
                }
            } else { // If no file was submitted, set default avatar
                string path = AppDomain.CurrentDomain.BaseDirectory + "/Content/Images/defaultAvatar.png";
                FileStream file = new FileStream(path, FileMode.Open);
                using(BinaryReader binary = new BinaryReader(file)) {
                    profile.ProfileImage = binary.ReadBytes((int)file.Length);
                }
            }
            // Fill out the data that is missing from the submitted model
            profile.Id = User.Identity.GetUserId();
            profile.IsActivated = false;
            profile.IsAdmin = false;
            profile.LastLogout = DateTime.Now;
            // Add profile to database
            profileRepository.Add(profile);
            profileRepository.Save();
            // Add user to role "Profiled", meaning that they have a profile.
            userRepository.AddUserToProfiledRole(User.Identity.GetUserId());
            // Set the DisplayName for the notification bar
            ApplicationUser user = userRepository.Get(User.Identity.GetUserId());
            user.LockoutEnabled = false;
            user.DisplayName = (profile.FirstName + " " + profile.LastName);
            userRepository.Edit(user);
            userRepository.Save();
            // Send user to their profile page
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Profiled")]
        public ActionResult Manage() {
            return View(profileRepository.Get(User.Identity.GetUserId())); // Manage Account for current user.
        }

        [Authorize(Roles = "Profiled")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Exclude = "ProfileImage")] ProfileModels updates) {
            // Excludes ProfileImage from controller call so the program does not crash
            if(!ModelState.IsValid) { // If model not valid
                return RedirectToAction("Manage"); // Return the original page
            }
            string currentUserId = User.Identity.GetUserId();
            // Get the existing profile
            ProfileModels profile = profileRepository.Get(currentUserId);
            // If nothing was changed
            if(
                profile.FirstName.Equals(updates.FirstName) &&
                profile.LastName.Equals(updates.LastName) &&
                profile.PhoneNumber.Equals(updates.PhoneNumber) &&
                profile.Title.Equals(updates.Title) &&
                profile.Description.Equals(updates.Description) &&
                Request.Files["ProfileImage"].ContentLength < 1
                ) {
                return RedirectToAction("Index");
            }
            // Set possibly edited values
            profile.PhoneNumber = updates.PhoneNumber;
            profile.Title = updates.Title;
            profile.Description = updates.Description;
            if(Request.Files["ProfileImage"].ContentLength >= 1) { // If a new profile image was submitted
                HttpPostedFileBase profileImgFile = Request.Files["ProfileImage"];
                using(BinaryReader binary = new BinaryReader(profileImgFile.InputStream)) {
                    profile.ProfileImage = binary.ReadBytes(profileImgFile.ContentLength); // Update the model's profile image to the new one
                }
            }
            // If there is no change in name, there is no need to change the display name.
            if(profile.FirstName.Equals(updates.FirstName) && profile.LastName.Equals(updates.LastName)) {
                // Edit profile in database
                profileRepository.Edit(profile);
                profileRepository.Save();
                return RedirectToAction("Index");
            }
            // If names do not match, edit names
            profile.FirstName = updates.FirstName;
            profile.LastName = updates.LastName;
            profileRepository.Edit(profile);
            profileRepository.Save();
            // Set the DisplayName for the notification bar
            ApplicationUser user = userRepository.Get(User.Identity.GetUserId());
            user.DisplayName = (updates.FirstName + " " + updates.LastName);
            userRepository.Edit(user);
            userRepository.Save();
            // Refresh their login token to update the IdentityUser's IdentityClaim in the AuthenticationCookie
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public FileContentResult RenderProfileImage(string userId) {
            // Converts the stored byte-array to an image. This action is called with razor in views to be used in img tags.
            object profileId = Request.RequestContext.RouteData.Values["id"];
            ProfileModels profile = null;
            if(!string.IsNullOrWhiteSpace(userId)) {
                profile = profileRepository.Get(userId);
            } else {
                profile = profileRepository.Get((string)profileId);
            }
            return new FileContentResult(profile.ProfileImage, "image/jpeg");
        }

        [Authorize(Roles = "Profiled")]
        [HttpPost]
        public ActionResult MakeAdmin(string Id) {
            ProfileModels profile = profileRepository.Get(Id);
            profile.IsAdmin = true;
            profileRepository.Edit(profile);
            profileRepository.Save();
            return Json(new { result = true });
        }

        [Authorize(Roles = "Profiled")]
        [HttpPost]
        public ActionResult RemoveAdmin(string Id) {
            ProfileModels profile = profileRepository.Get(Id);
            profile.IsAdmin = false;
            profileRepository.Edit(profile);
            profileRepository.Save();
            return Json(new { result = true });
        }

        [Authorize(Roles = "Profiled")]
        [HttpPost]
        public ActionResult BanUser(string Id) {
            ApplicationUser user = userRepository.Get(Id);
            user.LockoutEnabled = true;
            userRepository.Edit(user);
            userRepository.Save();
            return Json(new { result = true });
        }

        [Authorize(Roles = "Profiled")]
        [HttpPost]
        public ActionResult UnbanUser(string Id) {
            ApplicationUser user = userRepository.Get(Id);
            user.LockoutEnabled = false;
            userRepository.Edit(user);
            userRepository.Save();
            return Json(new { result = true });
        }
    }
}