using gestion_courrier_bo.Context;
using gestion_courrier_bo.Models;

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
            courrier.Recepteur = employe;
            courrier.Fichier = _fileUploadService.UploadFileAsync(formFile);
            List<CourrierDestinataire> destinataires = SelectedDestinataires
                .Select(departement => new CourrierDestinataire(courrier, departement)).ToList();
            _context.Courriers.Add(courrier);
            courrier.Destinataires = destinataires;
            return courrier;
        }

    }
}
