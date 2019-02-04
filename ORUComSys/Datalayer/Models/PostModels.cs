using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class PostModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PostFrom")]
        public string PostFromId { get; set; }
        public virtual ApplicationUser PostFrom { get; set; }
        [Required]
        public ForumType Forum { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual CategoryModels Category { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PostDateTime { get; set; }
        public byte[] AttachedFile { get; set; }
    }

    public enum ForumType {
        Formal,
        Informal
    }
}