using System;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades
{
	public class Genero: IId
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 40)]
        public string Nombre { get; set; }

        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}

