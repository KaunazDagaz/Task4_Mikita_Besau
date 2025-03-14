public interface IUserService
{
    Task<bool> IsCurrentUserValidAsync();
    Task<bool> IsCurrentUserIncludedAsync(List<int> userIds);
    Task UpdateUserStatusAsync(List<int> userIds, string status);
    Task RemoveUserAsync(List<int> userIds);
}