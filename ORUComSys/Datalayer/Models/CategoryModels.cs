using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Datalayer.Models {
    public class CategoryModels : IIdentifiable<int> {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}