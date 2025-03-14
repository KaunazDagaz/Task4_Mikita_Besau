namespace task4.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public required string Status { get; set; }
    }
}
