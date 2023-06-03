using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using gestion_courrier_bo.Context;
using gestion_courrier_bo.Models;
using gestion_courrier_bo.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace gestion_courrier_bo.Pages.courrier
{
    [Authorize(Roles = "REC")]
    public class CreateModel : PageModel
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IEmployeService _employeService;
        private  List<Departement> departements { get; set; }

        [BindProperty]
        public Courrier Courrier { get; set; } = default!;

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        [BindProperty]
        public List<string> SelectedDestinataires { get; set; }

        public CreateModel(gestion_courrier_bo.Context.AppDbContext context, 
            ICourrierService courrierService, 
            IEmployeService employeService)
        {
            _context = context;
            _courrierService = courrierService;
            _employeService = employeService;
            departements = _context.Departements.ToList();
        }

        public IActionResult OnGet()
        {
        ViewData["IdExpediteur"] = new SelectList(_context.Departements, "Id", "Nom");
        ViewData["IdFlag"] = new SelectList(_context.Flags, "Id", "Designation");
        ViewData["IdRecepteur"] = new SelectList(_context.Employes, "Id", "Nom");
        ViewData["Destinataires"] = new SelectList(_context.Departements, "Id", "Nom");
            return Page();
        }

       
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ClaimsPrincipal currentUser = User;
            //if (!ModelState.IsValid || _context.Courriers == null || Courrier == null)
            if (_context.Courriers == null || Courrier == null)
            {
                return Page();
            }

            string email = currentUser.Identity.Name;
            Employe connectedUser = _employeService.findEmployeByEmail(email);
            List<Departement> SelectedDepartements =  departements
                .Where(dept => SelectedDestinataires.Contains(dept.Id + "")).ToList();
            _courrierService.createCourrier(Courrier, connectedUser, SelectedDepartements, FileUpload);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Liste");
        }
    }
}
