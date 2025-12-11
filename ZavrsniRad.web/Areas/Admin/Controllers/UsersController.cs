using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZavrsniRad.Domain.Entities;
using ZavrsniRad.web.Data;
using ZavrsniRad.web.Services;          // zbog IEmailSender
using ZavrsniRad.web.Models.Admin;

namespace ZavrsniRad.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public UsersController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }

        // GET: /Admin/Users
        public async Task<IActionResult> Index()
        {
            var professors = await _context.Professors
                .AsNoTracking()
                .ToListAsync();

            return View(professors);
        }

        // GET: /Admin/Users/CreateProfessor
        public IActionResult CreateProfessor()
        {
            return View(new CreateProfessorViewModel());
        }

        // POST: /Admin/Users/CreateProfessor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfessor(CreateProfessorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1) Provjeri postoji li user s tim emailom
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Korisnik s ovom e-mail adresom već postoji.");
                return View(model);
            }

            // 2) Kreiraj IdentityUser BEZ lozinke
            var newUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = false
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // 3) Dodaj ga u rolu Professor
            await _userManager.AddToRoleAsync(newUser, "Professor");

            // 4) Kreiraj profesora u domeni
            var professor = new Professor
            {
                Id = newUser.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                AcademicTitle = model.AcademicTitle,
                Department = model.Department
            };

            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();

            // 5) Generiraj token i pošalji mail za postavljanje lozinke
            var token = await _userManager.GeneratePasswordResetTokenAsync(newUser);

            var callbackUrl = Url.Action(
                    "SetPassword",
                    "Account",
                    new { area = "", userId = newUser.Id, token = token },   // area = "" → root, ne Admin
                    protocol: HttpContext.Request.Scheme);


            var body = $@"
<p>Poštovani {model.FirstName} {model.LastName},</p>
<p>Kreiran vam je korisnički račun na platformi za završne radove.</p>
<p>Za aktivaciju računa i postavljanje lozinke kliknite na sljedeći link:</p>
<p><a href=""{callbackUrl}"">Postavi lozinku</a></p>";

            await _emailSender.SendEmailAsync(model.Email, "Aktivacija računa mentora", body);

            return RedirectToAction(nameof(Index));
        }
    }
}
