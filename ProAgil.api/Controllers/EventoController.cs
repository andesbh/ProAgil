using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Dominio;
using ProAgil.Repositorio;

namespace ProAgil.api.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    public class EventoController: ControllerBase
    {
        private IProAgilRepositorio _Repo;

        public EventoController(IProAgilRepositorio Repo)
        {
            _Repo = Repo;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
           
           try{
               var results = await _Repo.GetAllEventoAsync(true);
               return Ok(results);

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }
            
        }
        [HttpGet("{EventoID}")]
        public async Task<IActionResult> Get(int EventoID)
        {
           
           try{
               var results = await _Repo.GetEventoAsyncById(EventoID,true);
               return Ok(results);

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }
            
        }
        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
           
           try{
               var results = await _Repo.GetAllEventoAsyncByTema(tema, true);
               return Ok(results);

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }
            
        }

        [HttpPost()]
        public async Task<IActionResult> Post(Evento model)
        {
           
           try{
               _Repo.Add(model);
               if(await _Repo.SalveChangesAsync())
               {
                   return Created($"/api/evento{model.Id}", model);
               }

           }
           catch(Exception ex)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException);
           }

           return BadRequest();
            
        }

        [HttpPut()]
        public async Task<IActionResult> Put(int EventoID, Evento model)
        {
           
           try{

               var evento = await _Repo.GetEventoAsyncById(EventoID,false);
                if(evento == null) {return NotFound();}

               _Repo.Update(model);
               if(await _Repo.SalveChangesAsync())
               {
                   return Created($"/api/evento{model.Id}", model);
               }

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }

           return BadRequest();
            
        }
        [HttpDelete()]
        public async Task<IActionResult> Delete(int EventoID)
        {
           
           try{

               var evento = await _Repo.GetEventoAsyncById(EventoID,false);
                if(evento == null) {return NotFound();}

               _Repo.Delete(evento);
               if(await _Repo.SalveChangesAsync())
               {
                   return Ok();
               }

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }

           return BadRequest();
            
        }
    }
}