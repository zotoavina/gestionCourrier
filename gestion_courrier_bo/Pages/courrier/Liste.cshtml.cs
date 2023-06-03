
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using gestion_courrier_bo.Models;
using gestion_courrier_bo.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace gestion_courrier_bo.Pages.courrier
{
    [Authorize]
    public class ListeModel : PageModel
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IEmployeService _employeService;
        private readonly IConfiguration _configuration;
        private Employe _currentUser;
        public List<Employe> coursiers { get; set; }
        public IList<CourrierDestinataire> courrierDestinataire { get; set; } = default!;

        public ListeModel(gestion_courrier_bo.Context.AppDbContext context, ICourrierService courrierService,
            IEmployeService employeService, IConfiguration configuration)
        {
            _context = context;
            _courrierService = courrierService;
            _employeService = employeService;
            coursiers = _employeService.findEmployesByRole("COU");
            _configuration = configuration;
        }



        public async Task OnGetAsync()
        {
            string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
            string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
            string couRole = _configuration.GetValue<string>("Constants:Role:CouRole");
            string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
            ViewData["DirRole"] = dirRole;
            ViewData["SecRole"] = secRole;
            ViewData["CouRole"] = couRole;
            ViewData["RecRole"] = recRole;
            ViewData["Coursiers"] = new SelectList(coursiers, "Id", "Nom");
            
            if (_context.Courriers != null)
            {
                _currentUser = _employeService.findEmployeByClaim(User);
                courrierDestinataire = _courrierService.listeCourrier(_currentUser);
                int a = 0;
            }
        }

        public async Task<RedirectToPageResult> OnGetTransferer(int courrier, int destinataire,string role)
        {
            _courrierService.transfererCourrierSecDir(courrier,destinataire, role);
            return RedirectToPage("Liste");
        }
    }
}
