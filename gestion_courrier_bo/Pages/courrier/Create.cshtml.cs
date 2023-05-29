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

namespace gestion_courrier_bo.Pages.courrier
{
    public class CreateModel : PageModel
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;
        private readonly IFileUploadService _fileUploadService;

        public CreateModel(gestion_courrier_bo.Context.AppDbContext context, 
            IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public IActionResult OnGet()
        {
        ViewData["IdExpediteur"] = new SelectList(_context.Departements, "Id", "Nom");
        ViewData["IdFlag"] = new SelectList(_context.Flags, "Id", "Designation");
        ViewData["IdRecepteur"] = new SelectList(_context.Employes, "Id", "Nom");
        ViewData["Destinataires"] = new SelectList(_context.Departements, "Id", "Nom");
            return Page();
        }

        [BindProperty]
        public Courrier Courrier { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {


            //if (!ModelState.IsValid || _context.Courriers == null || Courrier == null)
            if (_context.Courriers == null || Courrier == null)
            {
                return Page();
            }

            var fileUpload = Request.Form["FileUpload"];

       

            _context.Courriers.Add(Courrier);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
