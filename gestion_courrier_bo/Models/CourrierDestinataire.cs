using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_courrier_bo.Models
{
    public class CourrierDestinataire
    {
        public int Id { get; set; }
        [ForeignKey("Courrier")]
        public int IdCourrier { get; set; }
        public Courrier Courrier { get; set; }

        [ForeignKey("Departement")]
        public int IdDestinataire { get; set; }
        public Departement Destinataire { get; set; }

        [ForeignKey("Employe")]
        public int? IdCoursier { get; set; }
        public Employe? Coursier { get; set; }

        [ForeignKey("StatusCourrier")]
        public int? IdStatus { get; set; }
        public StatusCourrier? Status { get; set; }
        public DateTime DateMaj { get; set; } = DateTime.Now;

        public CourrierDestinataire() { }   

        public CourrierDestinataire(Courrier courrier, Departement destinataire, StatusCourrier status)
        {
            this.Courrier = courrier;
            this.Destinataire = destinataire;
            this.Status = status;
        }

    }
}
