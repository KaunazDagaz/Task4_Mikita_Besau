using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using task4.Entities;

namespace task4.Utils
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsCurrentUserValidAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser != null && currentUser.Status != "Blocked";
        }

        public async Task<bool> IsCurrentUserIncludedAsync(List<int> userIds)
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser != null && userIds.Contains(currentUser.Id);
        }

        public async Task UpdateUserStatusAsync(List<int> userIds, string status)
        {
            foreach (var userId in userIds)
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    user.Status = status;
                }
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(List<int> userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    dbContext.Users.Remove(user);
                }
            }
            await dbContext.SaveChangesAsync();
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userEmail = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
            {
                return null;
            }
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        }
    }
}