using Microsoft.AspNetCore.Mvc;
using task4.Models;

public class AccountController : Controller
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var (success, errorMessage) = await accountService.RegisterUserAsync(model);
            if (!success)
            {
                AddModelError(model, errorMessage);
                return View(model);
            }
            return RedirectToAction("Index", "User");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var (success, errorMessage) = await accountService.LoginUserAsync(model);
            if (!success)
            {
                AddModelError(model, errorMessage);
                return View(model);
            }
            return RedirectToAction("Index", "User");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await accountService.SignOutAsync();
        return RedirectToAction("Login");
    }

    private void AddModelError<TModel>(TModel model, string? errorMessage)
    {
        if (errorMessage == "Invalid password.")
        {
            ModelState.AddModelError("Password", errorMessage);
        }
        else
        {
            ModelState.AddModelError("Email", errorMessage ?? string.Empty);
        }
    }
}
