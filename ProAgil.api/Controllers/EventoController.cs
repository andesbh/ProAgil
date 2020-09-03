using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.api.DTOs;
using ProAgil.Dominio;
using ProAgil.Repositorio;

namespace ProAgil.api.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    public class EventoController: ControllerBase
    {
        private IProAgilRepositorio _Repo;

        private IMapper _mapper;

        public EventoController(IProAgilRepositorio Repo, IMapper mapper)
        {
            _Repo = Repo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
           
           try{
               var eventos = await _Repo.GetAllEventoAsync(true);
               var results = _mapper.Map<EventoDTO[]>(eventos);

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
               var evento = await _Repo.GetEventoAsyncById(EventoID,true);
               var results = _mapper.Map<EventoDTO>(evento);
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
               var evento = await _Repo.GetAllEventoAsyncByTema(tema, true);
               var results = _mapper.Map<EventoDTO[]>(evento);
               return Ok(results);

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }
            
        }

        [HttpPost()]
        public async Task<IActionResult> Post(EventoDTO model)
        {
           
           try{
               var evento = _mapper.Map<Evento>(model);
               _Repo.Add(evento);

               if(await _Repo.SalveChangesAsync())
               {
                   return Created($"/api/evento{evento.Id}", _mapper.Map<EventoDTO>(evento));
               }

           }
           catch(Exception ex)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException);
           }

           return BadRequest();
            
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDTO model)
        {
           
           try{

               var evento = await _Repo.GetEventoAsyncById(EventoId,false);
                if(evento == null) {return NotFound();}

                _mapper.Map(model, evento);

               _Repo.Update(evento);
               if(await _Repo.SalveChangesAsync())
               {
                   return Created($"/api/evento{model.Id}", _mapper.Map<EventoDTO>(evento));
               }

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
           }

           return BadRequest();
            
        }
        [HttpDelete("{EventoId}")]
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

        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
           
           try{
               
               var file = Request.Form.Files[0];
               var folderName = Path.Combine("Resources", "Images");
               var pathToSalve = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0){
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSalve, fileName.Replace("\""," ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create)){
                        file.CopyTo(stream);
                    }
                }

               return Ok();
           }
           catch(Exception ex)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
           }

           return BadRequest("Erro ao tentar fazer o upload");
            
        }
    }
}