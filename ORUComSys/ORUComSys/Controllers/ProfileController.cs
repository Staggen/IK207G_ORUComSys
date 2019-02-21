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
            byte[] imageData = null;
            if(Request.Files["ProfileImage"].ContentLength >= 1) { // If a file was submitted
                HttpPostedFileBase profileImgFile = Request.Files["ProfileImage"];
                using(BinaryReader binary = new BinaryReader(profileImgFile.InputStream)) {
                    imageData = binary.ReadBytes(profileImgFile.ContentLength); // Set the profile image to the variable imageData
                }
            } else { // If no file was submitted, set default avatar
                string path = AppDomain.CurrentDomain.BaseDirectory + "/Content/Images/defaultAvatar.png";
                FileStream file = new FileStream(path, FileMode.Open);
                using(BinaryReader binary = new BinaryReader(file)) {
                    imageData = binary.ReadBytes((int)file.Length);
                }
            }
            // Set profile data
            profile.Id = User.Identity.GetUserId();
            profile.ProfileImage = imageData;
            profile.IsActivated = false;
            profile.LastLogout = DateTime.Now;
            // Add profile to database
            profileRepository.Add(profile);
            profileRepository.Save();
            // Add user to role "Profiled", meaning that they have a profile.
            userRepository.AddUserToProfiledRole(User.Identity.GetUserId());
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
        public ActionResult Manage([Bind(Exclude = "ProfileImage")] ProfileModels profile) {
            // Excludes ProfileImage from controller call so the program does not crash.
            if(ModelState.IsValid) { // If model is correct
                string currentUserId = User.Identity.GetUserId();
                ProfileModels reference = profileRepository.Get(profile.Id); // Required to not have all the non-edited data reset.
                byte[] imageData = null; // Holds possible new image
                if(Request.Files["ProfileImage"].ContentLength >= 1) { // If a file was submitted
                    HttpPostedFileBase profileImgFile = Request.Files["ProfileImage"];
                    using(BinaryReader binary = new BinaryReader(profileImgFile.InputStream)) {
                        imageData = binary.ReadBytes(profileImgFile.ContentLength); // Set the profile image to the variable imageData
                    }
                    profile.ProfileImage = imageData; // Set new profile image
                } else { // If no file was submitted
                    profile.ProfileImage = reference.ProfileImage; // Re-set old profile image
                }
                // Set non-edited values
                profile.IsActivated = reference.IsActivated;
                profile.IsAdmin = reference.IsAdmin;
                profile.LastLogout = reference.LastLogout;
                // Edit profile in database
                profileRepository.Edit(profile);
                profileRepository.Save();
                // Send user back to their profile page
                ViewBag.CurrentUserId = currentUserId;
                return RedirectToAction("Index");
            } else {
                return RedirectToAction("Manage");
            }
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