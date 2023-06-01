using System.ComponentModel.DataAnnotations;

namespace gestion_courrier_bo.Models
{
    public class StatusCourrier
    {
        public int Id { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public string code { get; set; }
    }
}
