using System.ComponentModel.DataAnnotations;

namespace gestion_courrier_bo.Models
{
    public class Flag
    {
        public int Id { get; set; }
        [Required]
        public String Designation { get; set; }
    }
}
