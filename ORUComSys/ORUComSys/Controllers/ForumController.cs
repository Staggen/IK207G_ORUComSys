using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    [Authorize(Roles = "Profiled")]
    public class ForumController : Controller {
        private AttachmentRepository attachmentRepository;
        private CategoryRepository categoryRepository;
        private CommentRepository commentRepository;
        private FollowingCategoryRepository followingCategoryRepository;
        private PostRepository postRepository;
        private ProfileRepository profileRepository;
        private ReactionRepository reactionRepository;
        private UserRepository userRepository;

        public ForumController() {
            ApplicationDbContext context = new ApplicationDbContext();
            attachmentRepository = new AttachmentRepository(context);
            categoryRepository = new CategoryRepository(context);
            commentRepository = new CommentRepository(context);
            followingCategoryRepository = new FollowingCategoryRepository(context);
            postRepository = new PostRepository(context);
            profileRepository = new ProfileRepository(context);
            reactionRepository = new ReactionRepository(context);
            userRepository = new UserRepository(context);
        }

        public ActionResult Index() { // Select which forum you wish to enter
            return View();
        }

        [HttpGet]
        public ActionResult Formal() { // Formal forum
            ViewBag.Title = "Formal";
            // Create a postViewModel with all the categories so the view has a model to loop through for the category checkboxes
            ForumViewModel forumViewModel = new ForumViewModel {
                Categories = categoryRepository.GetAll(),
                PartialViewModel = ConvertPostsToViewModels(ForumType.Formal)
            };
            return View("FormalForum", forumViewModel);
        }
        [HttpPost]
        public ActionResult Formal(ForumViewModel postViewModel) { // Formal forum
            int categoryId = (int)postViewModel.Category;
            if(categoryId.Equals(0)) {
                categoryId = 5;
            }
            string currentUserId = User.Identity.GetUserId();
            // Convert to PostModels.
            PostModels postModel = new PostModels {
                PostFromId = currentUserId,
                CategoryId = categoryId,
                Forum = ForumType.Formal,
                Content = postViewModel.Content,
                PostDateTime = DateTime.Now
            };
            // Add to database and save changes.
            postRepository.Add(postModel);
            postRepository.Save();
            // Get post so you can access it's ID.
            PostModels addedPost = postRepository.GetLastPostCreatedByProfileId(currentUserId);
            byte[] attachmentData = null;
            string filename = null;
            if(Request.Files["AttachedFile"].ContentLength >= 1) { // Check if a file is entered
                for(int i = 0; i < Request.Files.Count; i++) {
                    HttpPostedFileBase attachedFile = Request.Files[i];
                    filename = attachedFile.FileName.Substring(attachedFile.FileName.LastIndexOf(@"\") + 1);

                    using(var binary = new BinaryReader(attachedFile.InputStream)) {
                        // This is the byte-array we set as the ProfileImage property on the profile.
                        attachmentData = binary.ReadBytes(attachedFile.ContentLength);
                    }
                    // Make AttachmentModel
                    AttachmentModels attachmentModel = new AttachmentModels {
                        AttachedFile = attachmentData,
                        FileName = filename,
                        FileExtension = filename.Substring(filename.LastIndexOf(".")),
                        PostId = addedPost.Id
                    };
                    attachmentRepository.Add(attachmentModel);
                }
                attachmentRepository.Save();
            }
            // Send notification email
            List<string> FollowerIds = followingCategoryRepository.GetAllUserIdsByFollowedCategoryId(categoryId);
            ProfileModels sender = profileRepository.Get(currentUserId);
            List<AttachmentModels> attachments = attachmentRepository.GetAttachmentsByPostId(addedPost.Id);
            CategoryModels category = categoryRepository.Get(addedPost.CategoryId);
            foreach(string followerId in FollowerIds) {
                if(followerId.Equals(currentUserId)) {
                    continue; // Don't send emails to yourself about your own posts...
                }
                EmailViewModels emailModel = new EmailViewModels {
                    Sender = sender,
                    Recipient = profileRepository.Get(followerId),
                    Post = addedPost,
                    Attachments = attachments,
                    CategoryName = category.Name
                };
                string viewPath = "~/Views/Forum/NewPostNotificationEmail.cshtml";
                string recipient = userRepository.GetEmailByUserId(followerId);
                string subject = "New Post In Category You Follow - ORUComSys";
                EmailSupport.SendNotificationEmail(ControllerContext, viewPath, emailModel, recipient, subject);
            }

            return RedirectToAction("Formal");
        }

        [HttpGet]
        public ActionResult Informal() { // Informal forum
            ViewBag.Title = "Informal";
            // Create a postViewModel with all the categories so the view has a model to loop through for the category checkboxes
            ForumViewModel forumViewModel = new ForumViewModel {
                Categories = categoryRepository.GetAll(),
                PartialViewModel = ConvertPostsToViewModels(ForumType.Informal)
            };
            return View("InformalForum", forumViewModel);
        }

        [HttpPost]
        public ActionResult Informal(ForumViewModel postViewModel) { // Informal forum
            var categoryId = (int)postViewModel.Category;
            if(categoryId.Equals(0)) {
                categoryId = 5;
            }
            string currentUserId = User.Identity.GetUserId();
            // Convert to PostModels.
            PostModels postModel = new PostModels {
                PostFromId = currentUserId,
                CategoryId = categoryId,
                Forum = ForumType.Informal,
                Content = postViewModel.Content,
                PostDateTime = DateTime.Now
            };
            // Add to database and save changes.
            postRepository.Add(postModel);
            postRepository.Save();
            // Get post so you can access it's ID.
            PostModels addedPost = postRepository.GetLastPostCreatedByProfileId(currentUserId);
            byte[] attachmentData = null;
            string filename = null;
            if(Request.Files["AttachedFile"].ContentLength >= 1) { // Check if a file is entered
                for(int i = 0; i < Request.Files.Count; i++) {
                    HttpPostedFileBase attachedFile = Request.Files[i];
                    filename = attachedFile.FileName.Substring(attachedFile.FileName.LastIndexOf(@"\") + 1);

                    using(var binary = new BinaryReader(attachedFile.InputStream)) {
                        // This is the byte-array we set as the ProfileImage property on the profile.
                        attachmentData = binary.ReadBytes(attachedFile.ContentLength);
                    }
                    // Make AttachmentModel
                    AttachmentModels attachmentModel = new AttachmentModels {
                        AttachedFile = attachmentData,
                        FileName = filename,
                        FileExtension = filename.Substring(filename.LastIndexOf(".")),
                        PostId = addedPost.Id
                    };
                    attachmentRepository.Add(attachmentModel);
                }
                attachmentRepository.Save();
            }
            // Send notification email
            List<string> FollowerIds = followingCategoryRepository.GetAllUserIdsByFollowedCategoryId(categoryId);
            ProfileModels sender = profileRepository.Get(User.Identity.GetUserId());
            List<AttachmentModels> attachments = attachmentRepository.GetAttachmentsByPostId(addedPost.Id);
            CategoryModels category = categoryRepository.Get(addedPost.CategoryId);
            foreach(string followerId in FollowerIds) {
                if(followerId.Equals(currentUserId)) {
                    continue; // Don't send emails to yourself about your own posts...
                }
                EmailViewModels emailModel = new EmailViewModels {
                    Sender = sender,
                    Recipient = profileRepository.Get(followerId),
                    Post = addedPost,
                    Attachments = attachments,
                    CategoryName = category.Name
                };
                string viewPath = "~/Views/Forum/NewPostNotificationEmail.cshtml";
                string recipient = userRepository.GetEmailByUserId(followerId);
                string subject = "New Post In Category You Follow - ORUComSys";
                EmailSupport.SendNotificationEmail(ControllerContext, viewPath, emailModel, recipient, subject);
            }

            return RedirectToAction("Informal");
        }

        public PartialViewResult UpdatePosts(string Id) {
            // Check if the string argument is formal. If it is, set type to Formal, else set it to Informal.
            ForumType type = string.Equals(Id, "Formal", StringComparison.OrdinalIgnoreCase) ? ForumType.Formal : ForumType.Informal;
            return PartialView("_ForumPosts", ConvertPostsToViewModels(type));
        }

        public PartialViewResult GetReactionList(int Id) {
            List<ReactionModels> postReactions = reactionRepository.GetAllReactionsByPostId(Id);
            return PartialView("_ReactionList", postReactions);
        }

        public PostViewModelsForUsers ConvertPostsToViewModels(ForumType type) {
            string currentUserId = User.Identity.GetUserId();
            List<PostModels> allPosts = postRepository.GetAllPostsByForumType(type);
            List<PostViewModels> allPostViewModel = new List<PostViewModels>();
            foreach(var post in allPosts) {
                PostViewModels postViewModel = new PostViewModels {
                    Id = post.Id,
                    PostFrom = post.PostFrom,
                    Category = post.Category.Category,
                    Forum = post.Forum,
                    Content = post.Content,
                    PostDateTime = post.PostDateTime,
                    CurrentUser = profileRepository.Get(currentUserId),
                    Reactions = reactionRepository.GetAllReactionsByPostId(post.Id),
                    Categories = categoryRepository.GetAll(),
                    Attachments = attachmentRepository.GetAttachmentsByPostId(post.Id),
                    Comments = commentRepository.GetAllCommentsByPostId(post.Id)
                };
                allPostViewModel.Add(postViewModel);
            }
            PostViewModelsForUsers postViewModelForUsers = new PostViewModelsForUsers {
                Posts = allPostViewModel,
                CurrentUser = profileRepository.Get(currentUserId)
            };
            return postViewModelForUsers;
        }

        [HttpPost]
        public void SubscribeToPostCategories(FollowingCategoryViewModels vessel) {
            string currentUserId = User.Identity.GetUserId();
            // Get all post categories the user is currently subscribed to.
            List<FollowingCategoryModels> FollowedCategories = followingCategoryRepository.GetAllFollowedCategoriesByUserId(currentUserId);
            // Get all categories
            List<CategoryModels> AllCategories = categoryRepository.GetAll();
            if(vessel.CategoriesToFollow != null) {
                // Take the input array and make it into a list, as they are easier to work with.
                List<string> ActiveCategories = vessel.CategoriesToFollow.ToList();
                // Unsubscribe from the categories which are not in the active selection
                foreach(FollowingCategoryModels followedCategory in FollowedCategories) {
                    bool RemoveSubscription = true;
                    foreach(string activeCategory in ActiveCategories) {
                        // Check if the currently followed category name equals the actively selected category name.
                        if(AllCategories.Single(category => category.Id.Equals(followedCategory.CategoryId)).Name.Equals(activeCategory)) {
                            RemoveSubscription = false;
                        }
                    }
                    // If a followed category does not match the active categories, remove it.
                    if(RemoveSubscription) {
                        followingCategoryRepository.Remove(followedCategory.Id);
                    }
                }
                // Subscribe to new active categories which do not match the current subscriptions
                foreach(string activeCategory in ActiveCategories) {
                    bool AddSubscription = true;
                    foreach(FollowingCategoryModels followedCategory in FollowedCategories) {
                        if(activeCategory.Equals(AllCategories.Single(category => category.Id.Equals(followedCategory.CategoryId)).Name)) {
                            AddSubscription = false;
                        }
                    }
                    if(AddSubscription) {
                        foreach(CategoryModels category in AllCategories) {
                            if(activeCategory.Equals(category.Name)) {
                                FollowingCategoryModels model = new FollowingCategoryModels {
                                    ProfileId = currentUserId,
                                    CategoryId = category.Id
                                };
                                followingCategoryRepository.Add(model);
                            }
                        }
                    }
                }
            } else {
                // Unsubscribe from all post categories
                foreach(FollowingCategoryModels followedCategory in FollowedCategories) {
                    followingCategoryRepository.Remove(followedCategory.Id);
                }
            }
            // Save changes in the database
            followingCategoryRepository.Save();
        }

        public PartialViewResult GetAllCommentsByPostId(int id) {
            List<CommentModels> comments = commentRepository.GetAllCommentsByPostId(id);
            return PartialView("_ForumPostComments", comments);
        }

        [HttpPost]
        public ActionResult AddComment(CommentModels vessel) {
            if(!ModelState.IsValid) {
                return Json(new { result = false });
            }
            string currentUserId = User.Identity.GetUserId();
            CommentModels comment = new CommentModels {
                ProfileId = currentUserId,
                Content = vessel.Content,
                PostId = vessel.PostId,
                CommentDateTime = DateTime.Now
            };
            commentRepository.Add(comment);
            commentRepository.Save();
            return Json(new { result = true });
        }
    }
}