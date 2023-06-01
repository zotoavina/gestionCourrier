using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_courrier_bo.Models
{
    public class Courrier
    {
        public int Id { get; set; }
        public string Objet { get; set; }
        public string Reference { get; set; }
        public string? Commentaire { get; set; }
        public string? Fichier { get; set; }
        public DateTime DateReception { get; set; } = DateTime.Now;

        [ForeignKey("ExpediteurInterne")]
        public int? IdExpediteur { get; set; }
        public Departement? ExpediteurInterne { get; set; } = default!;

        public string? ExpediteurExterne { get; set; }

        [ForeignKey("Recepteur")]
        public int IdRecepteur { get; set; }
        public Employe Recepteur { get; set; } = default!;

        [ForeignKey("Flag")]
        public int IdFlag { get; set; }
        public Flag Flag { get; set; } = default!;

        public ICollection<CourrierDestinataire> Destinataires { get; set; }

    }
}
