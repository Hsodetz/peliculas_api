using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using PeliculasAPI.Helpers;
using System.Linq.Dynamic.Core;

namespace PeliculasAPI.Controllers
{
	[ApiController]
	[Route("/api/peliculas")]
	public class PeliculasController: CustomBaseController
	{
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly ILogger<PeliculasController> logger;
		private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext dbContext,
			IMapper mapper,
			IAlmacenadorArchivos almacenadorArchivos,
            ILogger<PeliculasController> logger): base(dbContext, mapper)

		{
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.logger = logger;
        }

		[HttpGet]
		public async Task<ActionResult<PeliculasIndexDTO>> Get()
		{
			var top = 5;
            var hoy = DateTime.Today;

            var proximosEstrenos = await dbContext.Peliculas
                .Where(x => x.FechaEstreno > hoy)
                .OrderBy(x => x.FechaEstreno)
                .Take(top)
                .ToListAsync();

            var enCines = await dbContext.Peliculas
                .Where(x => x.EnCines)
                .Take(top)
                .ToListAsync();

            var resultado = new PeliculasIndexDTO();
            resultado.FuturosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);
            return resultado;
		}

        [HttpGet("filtro")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] FiltroPeliculasDTO filtroPeliculasDTO)
        {
            var peliculasQueryable = dbContext.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(filtroPeliculasDTO.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(filtroPeliculasDTO.Titulo));
            }

            if (filtroPeliculasDTO.EnCines)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCines);
            }

            if (filtroPeliculasDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaEstreno > hoy);
            }

            if (filtroPeliculasDTO.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.PeliculasGeneros.Select(y => y.GeneroId)
                    .Contains(filtroPeliculasDTO.GeneroId));
            }

            // esta es una manera de ordenar, instalando el paquete System.Linq.Dynamic.Core y haciendo uso de using System.Linq.Dynamic.Core;
            if (!string.IsNullOrEmpty(filtroPeliculasDTO.CampoOrdenar))
            {
                var tipoOrden = filtroPeliculasDTO.OrdenAscendente ? "ascending" : "descending";
                try
                {
                    peliculasQueryable = peliculasQueryable.OrderBy($"{filtroPeliculasDTO.CampoOrdenar} {tipoOrden}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            }

            await HttpContext.InsertarParametrosPaginacion(peliculasQueryable,
                filtroPeliculasDTO.CantidadRegistrosPorPagina);

            var peliculas = await peliculasQueryable.Paginar(filtroPeliculasDTO.Paginacion).ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

		[HttpGet("{id:int}", Name = "obtenerPelicula")]
		public async Task<ActionResult<PeliculaDetallesDTO>> Get(int id)
		{
			Pelicula pelicula = await dbContext.Peliculas
                                .Include(x => x.PeliculasActores)
                                .ThenInclude(x => x.Actor)
                                .Include(x => x.PeliculasGeneros)
                                .ThenInclude(x => x.Genero)
                                .FirstOrDefaultAsync(pel => pel.Id == id);

			if(pelicula is null)
				return NotFound();

            pelicula.PeliculasActores = pelicula.PeliculasActores.OrderBy(x => x.Orden).ToList();

			PeliculaDetallesDTO peliculaDTO = mapper.Map<PeliculaDetallesDTO>(pelicula);

			return peliculaDTO;
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
		{
            Pelicula pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if(pelicula == null)
                return NotFound();

             if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }

            dbContext.Add(pelicula);
            await dbContext.SaveChangesAsync();

            PeliculaDTO peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return CreatedAtRoute("obtenerPelicula", new {id = peliculaDTO.Id}, peliculaDTO);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
		{
			var peliculaDB = await dbContext.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (peliculaDB == null) { return NotFound(); }

            peliculaDB = mapper.Map(peliculaCreacionDTO, peliculaDB);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        peliculaDB.Poster,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }

            await dbContext.SaveChangesAsync();
            return NoContent();
		}

		[HttpPatch]
		public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
		{
            return await Patch<Pelicula, PeliculaPatchDTO>(id, patchDocument);
		}

		[HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Pelicula>(id);
        }

	}
}

