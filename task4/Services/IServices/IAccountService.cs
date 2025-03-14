using task4.Models;

public interface IAccountService
{
    Task<(bool success, string? errorMessage)> RegisterUserAsync(RegisterViewModel model);
    Task<(bool success, string? errorMessage)> LoginUserAsync(LoginViewModel model);
    Task SignOutAsync();
}
