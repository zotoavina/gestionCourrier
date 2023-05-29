using gestion_courrier_bo.Models;

namespace gestion_courrier_bo.Services
{
    public interface ICourrierService
    {
        Courrier createCourrier(Courrier courrier, Employe employe, List<Departement> destinataires, IFormFile formFile);
    }
}
