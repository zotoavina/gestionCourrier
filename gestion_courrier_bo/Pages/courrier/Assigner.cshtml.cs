using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gestion_courrier_bo.Context;
using gestion_courrier_bo.Models;
using gestion_courrier_bo.Services;

namespace gestion_courrier_bo.Pages.courrier
{
    public class AssignerModel : PageModel
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;
        private readonly IEmployeService _employeService;

        private readonly ICourrierService _courrierService;
        public List<Employe> coursiers { get; set; }


        public AssignerModel(gestion_courrier_bo.Context.AppDbContext context, IEmployeService employeService,
            ICourrierService courrierService)
        {
            _context = context;
            _employeService = employeService;
            _courrierService = courrierService;
            coursiers = _employeService.findEmployesByRole("COU");
        }
        [BindProperty]
        public CourrierDestinataire CourrierDestinataire { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int courrier, int destinataire)
        {
            
            if (courrier == null || destinataire == null || _context.CourrierDestinataires == null || !CourrierDestinataireExists(courrier, destinataire))
            {
                return NotFound();
            }

            CourrierDestinataire = await _context.CourrierDestinataires
            .Include(c => c.Destinataire)
            .Include(c => c.Status)
            .Include(c => c.Courrier)
            .Where(c => c.IdCourrier == courrier && c.IdDestinataire == destinataire)
            .FirstOrDefaultAsync();

            ViewData["Coursiers"] = new SelectList(coursiers, "Id", "Nom");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (CourrierDestinataire.IdCoursier != null)
            {
                int idCoursier =(int) CourrierDestinataire.IdCoursier;
                StatusCourrier status = _context.Status.Where(s => s.code == "ECDL").First();
              
                CourrierDestinataire.Status = status;
                CourrierDestinataire.DateMaj = DateTime.Now;
                CourrierDestinataire.Coursier = _context.Employes.Find(CourrierDestinataire.IdCoursier);
                CourrierDestinataire.Destinataire = _context.Departements.Find(CourrierDestinataire.IdDestinataire);
                CourrierDestinataire.Courrier = _context.Courriers.Find(CourrierDestinataire.IdCourrier);

                _context.Attach(CourrierDestinataire).State = EntityState.Modified;
                _context.SaveChanges();
               
            }

            return RedirectToPage("Liste");
        }

        private bool CourrierDestinataireExists(int idCourrier, int idDestinataire)
        {
          return (_context.CourrierDestinataires?.Any(e => e.IdCourrier == idCourrier &&  e.IdDestinataire == idDestinataire)).GetValueOrDefault();
        }
    }
}
