using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize]
    public class ForumController : Controller {
        private PostRepository postRepository;
        private ProfileRepository profileRepository;

        public ForumController() {
            ApplicationDbContext context = new ApplicationDbContext();
            postRepository = new PostRepository(context);
            profileRepository = new ProfileRepository(context);
        }

        public ActionResult Index() { // Select which forum you wish to enter
            return View();
        }

        [HttpGet]
        public ActionResult Formal() { // Formal forum
            ViewBag.Title = "Formal";
            return View("FormalForum");
        }

        [HttpPost]
        public ActionResult Formal(PostViewModels postModel) { // Formal forum

            var requestUrl = Request.RawUrl;
            var forumType = requestUrl.Split(new string[] { "/Forum/" }, StringSplitOptions.RemoveEmptyEntries);
            
            // check modelstate.
            if (ModelState.IsValid) {
                // get current user id (the poster's id).
                string currentUser = User.Identity.GetUserId();

                byte[] attachmentData = null;
                if (Request.Files["AttachedFile"].ContentLength >= 1) { // Check if a file is entered
                    HttpPostedFileBase attachedFile = Request.Files["AttachedFile"];

                    using (var binary = new BinaryReader(attachedFile.InputStream)) {
                        //This is the byte-array we set as the ProfileImage property on the profile.
                        attachmentData = binary.ReadBytes(attachedFile.ContentLength);
                    }
                }

                // convert to FormalPostModel.
                PostModels modelToSave = new PostModels {
                    Id = postModel.Id,
                    PostFromId = currentUser,
                    CategoryId = 1,
                    Forum = ForumType.Formal,
                    Content = postModel.Content,
                    PostDateTime = DateTime.Now,
                    AttachedFile = attachmentData,
                };

                // add to db and save changes.
                postRepository.Add(modelToSave);
                postRepository.Save();
            }
            return RedirectToAction("Formal");
        }

        [HttpGet]
        public ActionResult Informal() { // Informal forum
            ViewBag.Title = "Informal";
            return View("InformalForum");
        }

        [HttpPost]
        public ActionResult Informal(PostViewModels postModel) { // Formal forum

            // check modelstate.
            if (ModelState.IsValid) {
                // get current user id (the poster's id).
                string currentUser = User.Identity.GetUserId();

                byte[] attachmentData = null;
                if (Request.Files["AttachedFile"].ContentLength >= 1) { // Check if a file is entered
                    HttpPostedFileBase attachedFile = Request.Files["AttachedFile"];

                    using (var binary = new BinaryReader(attachedFile.InputStream)) {
                        //This is the byte-array we set as the ProfileImage property on the profile.
                        attachmentData = binary.ReadBytes(attachedFile.ContentLength);
                    }
                }

                // convert to FormalPostModel.
                PostModels modelToSave = new PostModels {
                    Id = postModel.Id,
                    PostFromId = currentUser,
                    CategoryId = 1,
                    Forum = ForumType.Informal,
                    Content = postModel.Content,
                    PostDateTime = DateTime.Now,
                    AttachedFile = attachmentData,
                };

                // add to db and save changes.
                postRepository.Add(modelToSave);
                postRepository.Save();
            }
            return RedirectToAction("Informal");
        }

        public PartialViewResult UpdatePosts(string Id) {
            // Check if the string argument is formal. If it is, set type to Formal, else set it to Informal.
            ForumType type = string.Equals(Id, "Formal", System.StringComparison.OrdinalIgnoreCase) ? ForumType.Formal : ForumType.Informal;
            return PartialView("_ForumPosts", ConvertPostsToViewModels(type));
        }

        public PostViewModelsForUsers ConvertPostsToViewModels(ForumType type) {
            List<PostModels> allPosts = postRepository.GetAllPostsByForumType(type);
            List<PostViewModels> allPostViewModel = new List<PostViewModels>();
            foreach (var post in allPosts) {
                PostViewModels postViewModel = new PostViewModels {
                    Id = post.Id,
                    PostFrom = post.PostFrom,
                    Category = post.Category,
                    Forum = post.Forum,
                    Content = post.Content,
                    PostDateTime = post.PostDateTime,
                    CurrentUser = profileRepository.Get(User.Identity.GetUserId())
                };
                allPostViewModel.Add(postViewModel);
            }

            PostViewModelsForUsers postViewModelForUsers = new PostViewModelsForUsers {
                PostList = allPostViewModel,
                CurrentUser = profileRepository.Get(User.Identity.GetUserId())
            };

            return postViewModelForUsers;
        }
    }
}