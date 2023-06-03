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
        private readonly IConfiguration _configuration;

        public CourrierService(AppDbContext context, IFileUploadService fileUploadService, IConfiguration configuration)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
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
            StatusCourrier status = _context.Status.Where(s => s.code == _configuration["Constants:Status:Assigne"]).First();
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

        

        public IList<CourrierDestinataire> listeCourrier(Employe employe)
        {
            if(employe.Poste.code == _configuration["Constants:Role:RecRole"])
            {
                return listeCourrierReceptionniste();
            }
            if(employe.Poste.code == _configuration["Constants:Role:CouRole"])
            {
                return listeCourrierCoursier(employe);
            }
            if ((employe.Poste.code == _configuration["Constants:Role:SecRole"]) || (employe.Poste.code == _configuration["Constants:Role:DirRole"]))
            {
                return listeCourrierSecDir(employe);
            }
            return null;
        }

        private IList<CourrierDestinataire> listeCourrierReceptionniste()
        {
            return ListeCourrierBaseQuery().ToList();
        }

        private IList<CourrierDestinataire> listeCourrierCoursier(Employe employe)
        {
            return ListeCourrierBaseQuery()
                .Where(c => c.IdCoursier == employe.Id 
                 && c.Status.code == _configuration["Constants:Status:Assigne"])
                .ToList();
        }

        private IList<CourrierDestinataire> listeCourrierSecDir(Employe employe)
        {
            return ListeCourrierBaseQuery()
                .Where(c => c.IdDestinataire == employe.DepartementId 
                && (c.Status.code == _configuration["Constants:Status:LivDirecteur"]
                || c.Status.code == _configuration["Constants:Status:LivSecretaire"]))
                .ToList();
        }

        private IQueryable<CourrierDestinataire> ListeCourrierBaseQuery()
        {
            return _context.CourrierDestinataires
                .Include(c => c.Courrier)
                    .ThenInclude(c => c.Flag)
                .Include(c => c.Courrier)
                    .ThenInclude(c => c.ExpediteurInterne)
                .Include(c => c.Courrier)
                    .ThenInclude(c => c.Recepteur)
                .Include(c => c.Coursier)
                .Include(c => c.Destinataire)
                .Include(c => c.Status);
        }

        public void transfererCourrierSecDir(int courrier, int destinataire,string transferer)
        {
            CourrierDestinataire courrierDestinataire = findCourrierByKey(courrier,destinataire);
            if (transferer == _configuration["Constants:Role:SecRole"])
            {
                StatusCourrier status = _context.Status.Where(s => s.code == _configuration["Constants:Status:LivSecretaire"]).First();
                courrierDestinataire.Status = status;
              
            }
            else if (transferer == _configuration["Constants:Role:DirRole"])
            {
                StatusCourrier status = _context.Status.Where(s => s.code == _configuration["Constants:Status:LivDirecteur"]).First();
                courrierDestinataire.Status = status;
            }
           
            courrierDestinataire.DateMaj = DateTime.Now;
            _context.Attach(courrierDestinataire).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public CourrierDestinataire findCourrierByKey(int idCourrier, int idDestinataire)
        {
            CourrierDestinataire courrierDestinataire =  _context.CourrierDestinataires
                .Include(c => c.Destinataire)
                .Include(c => c.Status)
                .Include(c => c.Courrier)
                .Where(c => c.IdCourrier == idCourrier && c.IdDestinataire == idDestinataire)
                .FirstOrDefault();

            return courrierDestinataire;
        }
    }
}
