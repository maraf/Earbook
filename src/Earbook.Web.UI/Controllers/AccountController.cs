using Earbook.Models.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext repository;

        public AccountController(DataContext repository)
        {
            Ensure.NotNull(repository, "repository");
            this.repository = repository;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username)
        {
            if (string.IsNullOrEmpty(username))
                return View();

            await repository.EnsureAccountAsync(username);
            await repository.SaveChangesAsync();
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, username) }, CookieAuthenticationDefaults.AuthenticationScheme)));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
