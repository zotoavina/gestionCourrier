using gestion_courrier_bo.Models;
using System.Security.Claims;

namespace gestion_courrier_bo.Services
{
    public interface IEmployeService
    {
        Employe findEmployeByEmail(string email);

        List<Employe> findEmployesByRole(string posteCode);

        Employe findEmployeByClaim(ClaimsPrincipal currentUser);
    }
}
