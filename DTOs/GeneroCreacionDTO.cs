using System;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
	public class GeneroCreacionDTO
	{
		[Required]
        [StringLength(maximumLength: 40)]
		public string Nombre { get; set; }
	}
}

