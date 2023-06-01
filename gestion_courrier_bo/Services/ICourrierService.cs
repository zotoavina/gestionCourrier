using gestion_courrier_bo.JoinedModels;
using gestion_courrier_bo.Models;

namespace gestion_courrier_bo.Services
{
    public interface ICourrierService
    {
        Courrier createCourrier(Courrier courrier, Employe employe, List<Departement> destinataires, IFormFile formFile);
        List<CourrierDepartement> courrierList(Employe employe);

        CourrierDestinataire assignerCoursier(int idCoursier, int idCourrierDesti);

        IList<CourrierDestinataire> listeCourrier(Employe employe);
    }
}
