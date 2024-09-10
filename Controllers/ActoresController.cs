using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("/api/actores")]
    public class ActoresController: CustomBaseController
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext dbContext,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos): base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            // con paginacion
            return await Get<Actor, ActorDTO>(paginacionDTO);

            // sin paginacion
            // List<Actor> actores = await dbContext.Actores.ToListAsync();

            // var dtos = mapper.Map<List<ActorDTO>>(actores);

            // return dtos;
        }

        [HttpGet("{id:int}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            return await Get<Actor, ActorDTO>(id);  
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDto)
        {
            Actor actor = mapper.Map<Actor>(actorCreacionDto);

            if(actor == null)
                return NotFound();

             if (actorCreacionDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDto.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDto.Foto.FileName);
                    actor.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        actorCreacionDto.Foto.ContentType);
                }
            }

            dbContext.Add(actor);
            await dbContext.SaveChangesAsync();

            ActorDTO actorDTO = mapper.Map<ActorDTO>(actor);

            return CreatedAtRoute("obtenerActor", new {id = actorDTO.Id}, actorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await dbContext.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actorDB == null) { return NotFound(); }

            actorDB = mapper.Map(actorCreacionDTO, actorDB);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        actorDB.Foto,
                        actorCreacionDTO.Foto.ContentType);
                }
            }

            await dbContext.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            return await Patch<Actor, ActorPatchDTO>(id, patchDocument);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Actor>(id);
        }
        
    }
}