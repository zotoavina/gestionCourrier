using System.ComponentModel.DataAnnotations;

namespace gestion_courrier_bo.Models
{
    public class StatusCourrier
    {
        public int Id { get; set; }
        [Required]
        public String Label { get; set; }
    }
}
