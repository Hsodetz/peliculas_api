using System.ComponentModel.DataAnnotations;
using PeliculasAPI.Validaciones;

namespace PeliculasAPI.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(maximumLength: 120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento  { get; set; }
    }
}