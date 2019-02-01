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

        public ProfileController() {
            ApplicationDbContext context = new ApplicationDbContext();
            profileRepository = new ProfileRepository(context);
        }

        public ActionResult Index(string userId) {
            string currentUserId = User.Identity.GetUserId();
            ProfileModels profile = null;
            if(!string.IsNullOrWhiteSpace(userId)) {
                profile = profileRepository.Get(userId);
                ViewBag.ProfileId = userId;
                ViewBag.CurrentUserId = currentUserId;
            } else {
                profile = profileRepository.Get(currentUserId);
                ViewBag.ProfileId = currentUserId;
                ViewBag.CurrentUserId = currentUserId;
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
            if (ModelState.IsValid) { // If model is correct
                byte[] imageData = null;
                if (Request.Files["ProfileImage"].ContentLength >= 1) { // If a file was submitted
                    HttpPostedFileBase profileImgFile = Request.Files["ProfileImage"];
                    using (BinaryReader binary = new BinaryReader(profileImgFile.InputStream)) {
                        imageData = binary.ReadBytes(profileImgFile.ContentLength); // Set the profile image to the variable imageData
                    }
                } else { // If no file was submitted, set default avatar
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Content/Images/defaultAvatar.png";
                    FileStream file = new FileStream(path, FileMode.Open);
                    using (BinaryReader binary = new BinaryReader(file)) {
                        imageData = binary.ReadBytes((int)file.Length);
                    }
                }
                // Set profile data
                profile.Id = User.Identity.GetUserId();
                profile.ProfileImage = imageData;
                profile.IsActivated = false;
                // Add profile to database
                profileRepository.Add(profile);
                profileRepository.Save();
                // Send user to their profile page
                return RedirectToAction("Index");
            } else {
                return RedirectToAction("Create");
            }
        }

        public ActionResult Manage() {
            return View(profileRepository.Get(User.Identity.GetUserId())); // Manage Account for current user.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Exclude = "ProfileImage")] ProfileModels profile) {
            // Excludes ProfileImage from controller call so the program does not crash.
            if (ModelState.IsValid) { // If model is correct
                string currentUserId = User.Identity.GetUserId();
                byte[] profileImageBackup = profileRepository.Get(currentUserId).ProfileImage; // Create backup of current profile image, in case they don't add a new one
                byte[] imageData = null; // Holds possible new image
                if (Request.Files["ProfileImage"].ContentLength >= 1) { // If a file was submitted
                    HttpPostedFileBase profileImgFile = Request.Files["ProfileImage"];
                    using (BinaryReader binary = new BinaryReader(profileImgFile.InputStream)) {
                        imageData = binary.ReadBytes(profileImgFile.ContentLength); // Set the profile image to the variable imageData
                    }
                    profile.ProfileImage = imageData; // Set new profile image
                } else { // If no file was submitted
                    profile.ProfileImage = profileImageBackup; // Re-set old profile image
                }
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
            var profileId = Request.RequestContext.RouteData.Values["id"];
            ProfileModels profile = null;
            if (userId != null) {
                profile = profileRepository.Get(userId);
            } else {
                profile = profileRepository.Get((string)profileId);
            }

            return new FileContentResult(profile.ProfileImage, "image/jpeg");
        }
    }
}