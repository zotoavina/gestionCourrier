using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using gestion_courrier_bo.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Security.Policy;

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

            // Redirect to a protected page or perform any other desired action
            return RedirectToPage("/Index");
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
           return BCrypt.Net.BCrypt.Verify(password,passwordHash);
        }

        private string HashPassword(string password)
        {
            // Generate a salt for the password hash
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password using the generated salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }
    }

    }
