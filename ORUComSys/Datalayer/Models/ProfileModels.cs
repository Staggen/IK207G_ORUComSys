using Datalayer.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models {
    public class ProfileModels : IIdentifiable<string> {
        [Key]
        [Display(Name = "User Id")]
        [ForeignKey("User")]
        public string Id { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You need to have a first name.")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Your first name must be at least 1 character long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You need to have a last name.")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Your last name must be at least 1 character long.")]
        public string LastName { get; set; }

        [Display(Name = "Profile Image")]
        public byte[] ProfileImage { get; set; }

        public bool IsActivated { get; set; } = true; // Maybe not set to default here?

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }
    }
}