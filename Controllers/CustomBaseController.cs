using System;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;

namespace PeliculasAPI.Controllers
{
	public class CustomBaseController: ControllerBase
	{
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDbContext dbContext, IMapper mapper)
		{
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad: class
        {
            List<TEntidad> generos = await dbContext.Set<TEntidad>().AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(generos);

            return dtos;
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO)
            where TEntidad : class
        {
            var queryable = dbContext.Set<TEntidad>().AsQueryable();
            return await Get<TEntidad, TDTO>(paginacionDTO, queryable);
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO,
            IQueryable<TEntidad> queryable)
            where TEntidad : class
        {
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entidades);
        }

        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad: class, IId
        {
            var generoPorId = await dbContext.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
         
            if (generoPorId is null)
                return NotFound();

            var dto = mapper.Map<TDTO>(generoPorId);

            return dto;
        }

        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura>(TCreacion creacionDTO, string nombreRuta) where TEntidad: class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);

            dbContext.Add(entidad);
            await dbContext.SaveChangesAsync();

            var dtoLectura = mapper.Map<TLectura>(entidad);

            return CreatedAtRoute(nombreRuta, new { id = entidad.Id }, dtoLectura);
        }

        protected async Task<ActionResult> Put<TCreacion, TEntidad>(TCreacion creacionDTO, int id) where TEntidad:class, IId
        {
            var existe = await dbContext.Generos.AnyAsync(generoDTO => generoDTO.Id == id);

            if (!existe)
                return NotFound();

            var entidad = mapper.Map<TEntidad>(creacionDTO);
            entidad.Id = id;

            dbContext.Update(entidad);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Patch<TEntidad, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument) 
            where TDTO : class
            where TEntidad : class, IId
        {
            if (patchDocument is null)
                return BadRequest();

            var entidadDB = await dbContext.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB is null)
                return NotFound();

            var entidadDTO = mapper.Map<TDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
                return BadRequest(ModelState);

            mapper.Map(entidadDTO, entidadDB);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntidad>(int id) where TEntidad : class, IId, new()
        {
            var existe = await dbContext.Set<TEntidad>().AnyAsync(e => e.Id == id);

            if (!existe)
                return NotFound();

            dbContext.Remove(new TEntidad { Id = id });
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

	}
}

