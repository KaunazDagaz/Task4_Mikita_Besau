using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task4.Entities;
using task4.Models;

namespace task4.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(ApplicationDbContext dbContext, IUserService userService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var validationResult = await ValidateCurrentUserAsync();
            if (validationResult != null)
            {
                return validationResult;
            }
            var users = await dbContext.Users
                .OrderByDescending(u => u.LastLogin)
                .ToListAsync();
            var userViewModels = mapper.Map<List<UserViewModel>>(users);
            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Block([FromBody] List<int> userIds)
        {
            var validationResult = await ValidateCurrentUserAsync();
            if (validationResult != null)
            {
                return validationResult;
            }
            await userService.UpdateUserStatusAsync(userIds, "Blocked");
            validationResult = await ValidateCurrentUserIncludedAsync(userIds);
            if (validationResult != null)
            {
                return validationResult;
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Unblock([FromBody] List<int> userIds)
        {
            var validationResult = await ValidateCurrentUserAsync();
            if (validationResult != null)
            {
                return validationResult;
            }
            await userService.UpdateUserStatusAsync(userIds, "Active");
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] List<int> userIds)
        {
            var validationResult = await ValidateCurrentUserAsync();
            if (validationResult != null)
            {
                return validationResult;
            }
            await userService.RemoveUserAsync(userIds);
            validationResult = await ValidateCurrentUserIncludedAsync(userIds);
            if (validationResult != null)
            {
                return validationResult;
            }
            return Json(new { success = true });
        }

        private async Task<IActionResult?> ValidateCurrentUserAsync()
        {
            if (!await userService.IsCurrentUserValidAsync())
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        private async Task<IActionResult?> ValidateCurrentUserIncludedAsync(List<int> userIds)
        {
            if (await userService.IsCurrentUserIncludedAsync(userIds))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }
            return null;
        }
    }
}