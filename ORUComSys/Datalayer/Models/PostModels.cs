using Datalayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class PostModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("PostFrom")]
        public string PostFromId { get; set; }
        public virtual ProfileModels PostFrom { get; set; }
        
        public ForumType Forum { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual CategoryModels Category { get; set; }

        [Required]
        public string Content { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PostDateTime { get; set; }

        public byte[] AttachedFile { get; set; }
    }

    public enum ForumType {
        Formal,
        Informal
    }
}