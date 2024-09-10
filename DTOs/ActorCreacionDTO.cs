using System.ComponentModel.DataAnnotations;
using PeliculasAPI.Validaciones;

namespace PeliculasAPI.DTOs
{

    public class ActorCreacionDTO: ActorPatchDTO // Hereda las propieddades de ActorPatchDTO, q son nombre y fecha de nacimiento
    {
        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}