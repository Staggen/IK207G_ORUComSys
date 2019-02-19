using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using ORUComSys.Models;
using System.Web.Http;

namespace ORUComSys.Controllers {
    public class AjaxApiController : ApiController {
        private AttachmentRepository attachmentRepository;
        private PostRepository postRepository;
        private ReactionRepository reactionRepository;

        public AjaxApiController() {
            ApplicationDbContext context = new ApplicationDbContext();
            attachmentRepository = new AttachmentRepository(context);
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
                string currentUserId = User.Identity.GetUserId();
                bool ReactionExists = reactionRepository.ReactionExists(reaction.PostId, currentUserId);

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
                // If user already has a reaction on this post, edit it. Else add a new one.
                if (ReactionExists) {
                    ReactionModels existingReaction = reactionRepository.GetReactionByPostAndProfileId(reaction.PostId, currentUserId);
                    existingReaction.Reaction = reactionType;
                    reactionRepository.Edit(existingReaction);
                    reactionRepository.Save();
                } else {
                    ReactionModels reactionModel = new ReactionModels() {
                        ProfileId = currentUserId,
                        Reaction = reactionType,
                        PostId = reaction.PostId
                    };
                    reactionRepository.Add(reactionModel);
                    reactionRepository.Save();
                }
            }
        }

        [HttpGet]
        public byte[] GetAttachment(int id) {
            return attachmentRepository.GetAttachmentByteArrayById(id);
        }
    }
}