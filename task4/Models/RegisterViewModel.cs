using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace task4.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public required string Password { get; set; }
    }
}