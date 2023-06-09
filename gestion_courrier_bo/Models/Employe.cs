﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_courrier_bo.Models
{
    public class Employe
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }

        [ForeignKey("Poste")]
        public int PostId { get; set; }
        public Poste Poste { get; set; } = default!;


        [ForeignKey("Departement")]
        public int DepartementId { get; set; }

        public Departement Departement { get; set; } = default!;
        public string MotDePasse { get; set; }
    }
}
