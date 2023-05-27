using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_courrier_bo.Models
{
    public class HistoriqueCourrierDestinataire
    {
        public int Id { get; set; }
        [ForeignKey("CourrierDestinataire")]
        public int IdCourrierDestinataire { get; set; }
        public CourrierDestinataire CourrierDesti { get; set; }

        [ForeignKey("StatusCourrier")]
        public int IdStatus { get; set; }
        public StatusCourrier StatusCourrier { get; set; }
        public DateTime DateHistorique { get; set; }

    }
}
