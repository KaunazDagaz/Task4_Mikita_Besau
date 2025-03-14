using Microsoft.EntityFrameworkCore;

namespace task4.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public async Task<(User? user, string? error)> ValidateUserAsync(string email)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return (null, "Invalid email.");
            }
            if (user.Status == "Blocked")
            {
                return (null, "Ooops! Looks like you are blocked.");
            }
            return (user, null);
        }
    }
}
