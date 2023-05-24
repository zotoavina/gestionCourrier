using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using gestion_courrier_bo.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using gestion_courrier_bo.Models;

namespace gestion_courrier_bo.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Optional: Add any code needed when the login page is loaded
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            // Perform validation and error handling as needed

            // Find the user by username
            var employee = await _context.Employes.FirstOrDefaultAsync(u => u.Email == email);
            employee.Poste = _context.Postes.Find(employee.PostId);

            // Check if the user exists
            if (employee == null)
            {
                ModelState.AddModelError("Username", "Invalid username or password.");
                return Page();
            }

            // Verify the password
            if (!VerifyPassword(password, employee.MotDePasse))
            {
                ModelState.AddModelError("Username", "Invalid username or password.");
                return Page();
            }

            // Login successful, perform any necessary actions (e.g., set authentication cookie)

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Email),
                new Claim(ClaimTypes.Role, employee.Poste.Nom),
                // Add other role claims as needed
            };

            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimsIdentity));

            // Redirect to a protected page or perform any other desired action
            return RedirectToPage("/Index");
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
           return BCrypt.Net.BCrypt.Verify(password,passwordHash);
        }

        
    }

    }
