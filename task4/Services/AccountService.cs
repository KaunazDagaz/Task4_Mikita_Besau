using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using task4.Entities;
using task4.Models;
using Isopoh.Cryptography.Argon2;

public class AccountService : IAccountService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;
    private readonly ApplicationDbContext dbContext;

    public AccountService(IHttpContextAccessor httpContextAccessor, IMapper mapper, ApplicationDbContext dbContext)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
        this.dbContext = dbContext;
    }

    public async Task<(bool success, string? errorMessage)> RegisterUserAsync(RegisterViewModel model)
    {
        var user = mapper.Map<User>(model);
        dbContext.Users.Add(user);
        var saveResult = await SaveChangesAsync();
        if (!saveResult.success)
        {
            return (false, saveResult.errorMessage);
        }
        await SignInAsync(user);
        return (true, null);
    }

    public async Task<(bool success, string? errorMessage)> LoginUserAsync(LoginViewModel model)
    {
        var (user, error) = await ValidateUserAsync(model.Email);
        if (error != null)
        {
            return (false, error);
        }
        if (!Argon2.Verify(user!.PasswordHash, model.Password))
        {
            return (false, "Invalid password.");
        }
        await UpdateLastLoginAsync(user!);
        await SignInAsync(user!);
        return (true, null);
    }

    public async Task SignOutAsync()
    {
        if (httpContextAccessor.HttpContext != null)
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    private async Task<(bool success, string? errorMessage)> SaveChangesAsync()
    {
        try
        {
            await dbContext.SaveChangesAsync();
            return (true, null);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("duplicate key") == true)
        {
            return (false, "Email is already taken.");
        }
    }

    private async Task UpdateLastLoginAsync(User user)
    {
        user.LastLogin = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    private async Task SignInAsync(User user)
    {
        if (httpContextAccessor.HttpContext != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }

    private async Task<(User? user, string? error)> ValidateUserAsync(string email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return (null, "Invalid email.");
        }
        else if (user.Status == "Blocked")
        {
            return (null, "Ooops! Looks like you are blocked.");
        }
        return (user, null);
    }
}