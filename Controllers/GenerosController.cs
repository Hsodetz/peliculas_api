using System;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("/api/generos")]
	public class GenerosController: CustomBaseController
	{
     

        public GenerosController(ApplicationDbContext dbContext,
            IMapper mapper): base(dbContext, mapper)
        {
        }


        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            return await Get<Genero, GeneroDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            return await Get<Genero, GeneroDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "obtenerGenero");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] GeneroCreacionDTO generoCreacionDTO, int id)
        {
            return await Put<GeneroCreacionDTO, Genero>(generoCreacionDTO, id);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Genero>(id);
        }



	}



}

