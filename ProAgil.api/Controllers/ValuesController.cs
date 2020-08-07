using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repositorio;

namespace ProAgil.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ProAgilContext  _contexto { get; }

        public ValuesController(ProAgilContext Contexto)
        {
            this._contexto = Contexto;
        }


        // // GET api/values
        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
           
        // //    try{
        // //        var results = await _contexto.Eventos.ToListAsync();
        // //        return Ok(results);

        // //    }
        // //    catch(Exception ex)
        // //    {
        // //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
        // //    }

        // return NotFound();
            
        // }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
