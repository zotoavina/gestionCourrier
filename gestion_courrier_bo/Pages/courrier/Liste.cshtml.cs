using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using gestion_courrier_bo.Context;
using gestion_courrier_bo.Models;
using gestion_courrier_bo.Services;
using gestion_courrier_bo.JoinedModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestion_courrier_bo.Pages.courrier
{
    public class ListeModel : PageModel
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IEmployeService _employeService;

        public List<Employe> coursiers { get; set; }
        public IList<Courrier> Courrier { get; set; } = default!;

        public ListeModel(gestion_courrier_bo.Context.AppDbContext context, ICourrierService courrierService,
            IEmployeService employeService)
        {
            _context = context;
            _courrierService = courrierService;
            _employeService = employeService;
            coursiers = _employeService.findEmployesByRole("COU");
        }



        public async Task OnGetAsync()
        {
            ViewData["Coursiers"] = new SelectList(coursiers, "Id", "Nom");
            if (_context.Courriers != null)
            {
                Courrier = await _context.Courriers
                .Include(c => c.Destinataires).ThenInclude(d => d.Destinataire)
                .Include(c => c.Destinataires).ThenInclude(d => d.Status)
                .Include(c => c.ExpediteurInterne)
                .Include(c => c.Flag)
                .Include(c => c.Recepteur).ToListAsync();
                int a = 0;
            }
        }
    }
}
