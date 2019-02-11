using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class ReactionModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ProfileModels User { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual PostModels Post { get; set; }

        public ReactionType Reaction { get; set; }
    }

    public enum ReactionType {
        Like,
        Love,
        Hate,
        XD
    }
}