using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using gestion_courrier_bo.Context;
using gestion_courrier_bo.Models;

namespace gestion_courrier_bo.Pages.Register
{
    public class RegisterModel : PageModel
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;

        public RegisterModel(gestion_courrier_bo.Context.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["DepartementId"] = new SelectList(_context.Departements, "Id", "Nom");
        ViewData["PostId"] = new SelectList(_context.Postes, "Id", "Nom");
            return Page();
        }

        [BindProperty]
        public Employe Employe { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

           Employe.Poste = _context.Postes.Find(Employe.PostId);
           Employe.Departement = _context.Departements.Find(Employe.DepartementId);

            //!ModelState.IsValid
            if (_context.Employes == null || Employe == null)
            {
                return Page();
            }

            Employe.MotDePasse = HashPassword(Employe.MotDePasse);
            _context.Employes.Add(Employe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./../Login/Login");
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
