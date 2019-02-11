using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Web.Http;

namespace ORUComSys.Controllers {
    public class AjaxApiController : ApiController {
        private ProfileRepository profileRepository;
        private UserRepository userRepository;
        private PostRepository postRepository;
        private ReactionRepository reactionRepository;

        public AjaxApiController() {
            ApplicationDbContext context = new ApplicationDbContext();
            profileRepository = new ProfileRepository(context);
            userRepository = new UserRepository(context);
            postRepository = new PostRepository(context);
            reactionRepository = new ReactionRepository(context);
        }

        [HttpDelete]
        public void DeletePost(int id) {
            postRepository.Remove(id);
            postRepository.Save();
        }

        [HttpPost]
        public void AddReaction(ReactionViewModels reaction) {
            if (ModelState.IsValid) {
                ReactionType reactionType = ReactionType.Like;
                var currentUser = User.Identity.GetUserId();
                var existingReactionBool = reactionRepository.GetReactionByPostAndUserIdBool(reaction.PostId, currentUser);

                switch (reaction.Reaction) {
                    case "like":
                        reactionType = ReactionType.Like;
                        break;
                    case "love":
                        reactionType = ReactionType.Love;
                        break;
                    case "hate":
                        reactionType = ReactionType.Hate;
                        break;
                    case "xd":
                        reactionType = ReactionType.XD;
                        break;
                    default:
                        break;
                }

                // If user already has a reaction on this post, edit it. else add a new one.
                if (existingReactionBool) {
                    var existingReaction = reactionRepository.GetReactionByPostAndUserId(reaction.PostId, currentUser);
                    existingReaction.Reaction = reactionType;
                    reactionRepository.Edit(existingReaction);
                    reactionRepository.Save();
                } else {
                    ReactionModels reactionModel = new ReactionModels() {
                        ProfileId = currentUser,
                        Reaction = reactionType,
                        PostId = reaction.PostId
                    };
                    reactionRepository.Add(reactionModel);
                    reactionRepository.Save();
                }
            }
        }
    }
}