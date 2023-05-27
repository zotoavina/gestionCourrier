﻿using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_courrier_bo.Models
{
    public class Courrier
    {
        public int Id { get; set; }
        public string Objet { get; set; }
        public string Reference { get; set; }
        public string? Commentaire { get; set; }
        public string? Fichier { get; set; } 
        public DateTime DateReception { get; set; }

        [ForeignKey("Departement")]
        public int? IdExpediteur { get; set; }
        public Departement? ExpediteurInterne { get; set; }

        public string? ExpediteurExterne { get; set; }

        [ForeignKey("Employe")]
        public int IdRecepteur { get; set; }
        public Employe Recepteur { get; set; }

        [ForeignKey("Flag")]
        public int IdFlag { get; set; }
        public Flag Flag { get; set; }

        public ICollection<CourrierDestinataire> Destinataires { get; set; }

    }
}
