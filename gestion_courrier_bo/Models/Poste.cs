using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_courrier_bo.Models
{
    public class Poste
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public String Nom { get; set; }

        [Required]  
        public string code { get; set; }

    }
}
