using System.ComponentModel.DataAnnotations;

namespace task4.Entities
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public DateTime? LastLogin { get; set; }
        [Required]
        public string Status { get; set; } = "Active";
    }
}
