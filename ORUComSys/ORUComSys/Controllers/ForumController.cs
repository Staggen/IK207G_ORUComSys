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
        private ReactionRepository reactionRepository;
        private CategoryRepository categoryRepository;
        private FollowingCategoryRepository followingCategoryRepository;

        public ForumController() {
            ApplicationDbContext context = new ApplicationDbContext();
            postRepository = new PostRepository(context);
            profileRepository = new ProfileRepository(context);
            reactionRepository = new ReactionRepository(context);
            categoryRepository = new CategoryRepository(context);
            followingCategoryRepository = new FollowingCategoryRepository(context);
        }

        public ActionResult Index() { // Select which forum you wish to enter
            return View();
        }

        [HttpGet]
        public ActionResult Formal() { // Formal forum
            ViewBag.Title = "Formal";

            PostViewModels postViewModel = new PostViewModels {
                Categories = categoryRepository.GetAll()
            };

            return View("FormalForum", postViewModel);
        }
        [HttpPost]
        public ActionResult Formal(PostViewModels postModel) { // Formal forum

            //var requestUrl = Request.RawUrl;  ////Probably a relic from when forums shared views.
            //var forumType = requestUrl.Split(new string[] { "/Forum/" }, StringSplitOptions.RemoveEmptyEntries);
            var catId = (int)postModel.Category;

            if (catId.Equals(0)) {
                catId = 5;
                postModel.Category = CategoryType.Other;
            }

            // check modelstate. REMOVED TO GET DEAFULT CATEGORY ASSIGNMENT TO WORK FOR NOW.

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
                CategoryId = catId,
                Forum = ForumType.Formal,
                Content = postModel.Content,
                PostDateTime = DateTime.Now,
                AttachedFile = attachmentData,
            };

            // add to db and save changes.
            postRepository.Add(modelToSave);
            postRepository.Save();

            return RedirectToAction("Formal");
        }

        [HttpGet]
        public ActionResult Informal() { // Informal forum
            ViewBag.Title = "Informal";

            // Create a postViewModel with all the categories so the view has a model to loop through for the category checkboxes
            PostViewModels postViewModel = new PostViewModels {
                Categories = categoryRepository.GetAll()
            };
            return View("InformalForum", postViewModel);
        }

        [HttpPost]
        public ActionResult Informal(PostViewModels postModel) { // Formal forum

            //var requestUrl = Request.RawUrl;  ////Probably a relic from when forums shared views.
            //var forumType = requestUrl.Split(new string[] { "/Forum/" }, StringSplitOptions.RemoveEmptyEntries);
            var catId = (int)postModel.Category;

            if (catId.Equals(0)) {
                catId = 5;
                postModel.Category = CategoryType.Other;
            }
            // check modelstate. REMOVED TO GET DEAFULT CATEGORY ASSIGNMENT TO WORK FOR NOW.

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
                CategoryId = catId,
                Forum = ForumType.Informal,
                Content = postModel.Content,
                PostDateTime = DateTime.Now,
                AttachedFile = attachmentData,
            };

            // add to db and save changes.
            postRepository.Add(modelToSave);
            postRepository.Save();

            return RedirectToAction("Informal");
        }

        public PartialViewResult UpdatePosts(string Id) {
            // Check if the string argument is formal. If it is, set type to Formal, else set it to Informal.
            ForumType type = string.Equals(Id, "Formal", System.StringComparison.OrdinalIgnoreCase) ? ForumType.Formal : ForumType.Informal;
            return PartialView("_ForumPosts", ConvertPostsToViewModels(type));
        }

        public PartialViewResult GetReactionList(int Id) {
            var postReactions = reactionRepository.GetAllReactionsByPostId(Id);
            return PartialView("_ReactionList", postReactions);
        }

        public PostViewModelsForUsers ConvertPostsToViewModels(ForumType type) {
            List<PostModels> allPosts = postRepository.GetAllPostsByForumType(type);
            List<PostViewModels> allPostViewModel = new List<PostViewModels>();
            foreach (var post in allPosts) {
                PostViewModels postViewModel = new PostViewModels {
                    Id = post.Id,
                    PostFrom = post.PostFrom,
                    Category = post.Category.Category,
                    Forum = post.Forum,
                    Content = post.Content,
                    PostDateTime = post.PostDateTime,
                    CurrentUser = profileRepository.Get(User.Identity.GetUserId()),
                    Reactions = reactionRepository.GetAllReactionsByPostId(post.Id),
                    Categories = categoryRepository.GetAll()
                };
                allPostViewModel.Add(postViewModel);
            }

            PostViewModelsForUsers postViewModelForUsers = new PostViewModelsForUsers {
                PostList = allPostViewModel,
                CurrentUser = profileRepository.Get(User.Identity.GetUserId())
            };

            return postViewModelForUsers;
        }

        // action result to receive ajax call
        [HttpPost]
        public ActionResult SubscribeToCategories(FollowingCategoryViewModels activeCategorySubVessel) {
            if (ModelState.IsValid) {
                var userId = User.Identity.GetUserId();

                // prepare a list of string to receive the string array of category names submitted in the view model.
                List<string> fixedActiveCategories = new List<string>();

                // loop the incoming model's string array and remove any nulled indexes in order to prevent crashing later on.
                foreach (var category in activeCategorySubVessel.CategoriesToSubscribe) {
                    if (category != null) {
                        fixedActiveCategories.Add(category);
                    }
                }

                // get all previously subscribed categories by the current user, so we can delete them all, in order to prevent redundant subscriptions.
                var allCategoriesCurrentlyFollowedByUser = followingCategoryRepository.GetAllFollowedCategoriesByUserId(userId); // this could also possibly be used in order to create a fail-safe operation later on.

                // delete them ALL!
                foreach (var category in allCategoriesCurrentlyFollowedByUser) {
                    followingCategoryRepository.Remove(category.Id);
                }
               
                // get a list of all registered categories.
                var allCategories = categoryRepository.GetAll();

                // loop all categories and match the name from the fixed list, in order to extrapolate the correct category id from the list of all categories for a new subscription. #coherentshitthesequel
                foreach (var cat in fixedActiveCategories) {
                    int catId = 0;
                    foreach (var category in allCategories) {
                        if (cat.Equals(category.Name)) { // if the categories match, create a model, set the variables...
                            catId = category.Id;
                            FollowingCategoryModels modelToSave = new FollowingCategoryModels {
                                ProfileId = User.Identity.GetUserId(),
                                CategoryId = catId
                            };
                            followingCategoryRepository.Add(modelToSave); // ...and add to the database.
                        }
                    }
                }
                followingCategoryRepository.Save(); // save all added subscriptions and return success to the ajax call...
                return Json(new { result = true });
            }
            return Json(new { result = false }); // ...or fu if it fails.
        }
    }
}