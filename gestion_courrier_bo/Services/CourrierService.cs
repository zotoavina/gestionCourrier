using gestion_courrier_bo.Context;
using gestion_courrier_bo.JoinedModels;
using gestion_courrier_bo.Models;
using Microsoft.EntityFrameworkCore;

namespace gestion_courrier_bo.Services
{
    public class CourrierService : ICourrierService
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;
        private readonly IFileUploadService _fileUploadService;

        public CourrierService(AppDbContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public Courrier createCourrier(Courrier courrier, Employe employe,
            List<Departement> SelectedDestinataires, IFormFile formFile){
            StatusCourrier statusCreer = _context.Status.Where(s => s.code == "REC").First();
            courrier.Recepteur = employe;
            courrier.Fichier = _fileUploadService.UploadFileAsync(formFile);
            List<CourrierDestinataire> destinataires = SelectedDestinataires
                .Select(departement => new CourrierDestinataire(courrier, departement, statusCreer)).ToList();
            _context.Courriers.Add(courrier);
            courrier.Destinataires = destinataires;
            return courrier;
        }

        public List<CourrierDepartement> courrierList(Employe employe)
        {
            var listeCourriers = from cr in _context.Courriers
                     join crdept in _context.CourrierDestinataires on cr.Id equals crdept.IdCourrier
                     join dept in _context.Departements on crdept.IdDestinataire equals dept.Id
                     select new CourrierDepartement { Courrier = cr, Destination = crdept, Departement = dept };
            return listeCourriers.ToList();
        }

        public CourrierDestinataire assignerCoursier(int idCoursier, int idCourrierDesti)
        {
            StatusCourrier status = _context.Status.Where(s => s.code == "ECDL").First();
            CourrierDestinataire courrierDestinataire = _context.CourrierDestinataires
                .Include(c => c.Destinataire)
                .Include(c => c.Status)
                .Include(c => c.Courrier)
                .Where(c => c.Id == idCourrierDesti)
                .FirstOrDefault();
            courrierDestinataire.Status = status;
            courrierDestinataire.DateMaj = DateTime.Now;
            courrierDestinataire.Coursier = _context.Employes.Find(idCoursier);
            _context.CourrierDestinataires.Update(courrierDestinataire);
            _context.SaveChanges();
            return courrierDestinataire;
        }

        public IList<Courrier> listeCourrier(Employe employe)
        {
            return null;
        }

        private IList<Courrier> listeCourrierReceptionniste()
        {
            IList<Courrier> courriers = new List<Courrier>();
            courriers = _context.Courriers
                 .Include(c => c.Destinataires).ThenInclude(d => d.Destinataire)
                 .Include(c => c.Destinataires).ThenInclude(d => d.Status)
                 .Include(c => c.ExpediteurInterne)
                 .Include(c => c.Flag)
                 .Include(c => c.Recepteur).ToList();
            return courriers;
        }

        private IList<Courrier> listeCourrierCoursier(Employe employe)
        {
            IList<Courrier> courriers = new List<Courrier>();
            courriers = _context.Courriers
                 .Include(c => c.Destinataires).ThenInclude(d => d.Destinataire)
                 .Include(c => c.Destinataires).ThenInclude(d => d.Status)
                 .Include(c => c.ExpediteurInterne)
                 .Include(c => c.Flag)
                 .Include(c => c.Recepteur).ToList();
            return courriers;
        }

        private IList<Courrier> listeCourrierSecDir(Employe employe)
        {
            return null;
        }


    }
}
