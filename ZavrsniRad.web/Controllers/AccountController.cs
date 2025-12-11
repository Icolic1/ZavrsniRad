using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZavrsniRad.web.Models;

namespace ZavrsniRad.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Account/SetPassword?userId=...&token=...
        [HttpGet]
        public IActionResult SetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var model = new SetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        // POST: /Account/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Korisnik ne postoji.");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // potvrdi e-mail nakon uspješnog postavljanja lozinke
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            // redirect na login
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
