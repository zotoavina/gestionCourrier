using gestion_courrier_bo.Models;

namespace gestion_courrier_bo.Services
{
    public interface IEmployeService
    {
        Employe findEmployeByEmail(string email);

        List<Employe> findEmployesByRole(string posteCode);
    }
}
