using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteDistribuidorsController : ControllerBase
    {
        private readonly DataContext _context;

        public ClienteDistribuidorsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ClienteDistribuidors
        [HttpGet]
        public IEnumerable<ClienteDistribuidor> GetClienteDistribuidor()
        {
            return _context.ClienteDistribuidor;
        }

        // GET: api/ClienteDistribuidors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteDistribuidor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteDistribuidor = await _context.ClienteDistribuidor.FindAsync(id);

            if (clienteDistribuidor == null)
            {
                return NotFound();
            }

            return Ok(clienteDistribuidor);
        }

        // PUT: api/ClienteDistribuidors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClienteDistribuidor([FromRoute] int id, [FromBody] ClienteDistribuidor clienteDistribuidor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clienteDistribuidor.ClienteId)
            {
                return BadRequest();
            }

            _context.Entry(clienteDistribuidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteDistribuidorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClienteDistribuidors
        [HttpPost]
        public async Task<IActionResult> PostClienteDistribuidor([FromBody] ClienteDistribuidor clienteDistribuidor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ClienteDistribuidor.Add(clienteDistribuidor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClienteDistribuidorExists(clienteDistribuidor.ClienteId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClienteDistribuidor", new { id = clienteDistribuidor.ClienteId }, clienteDistribuidor);
        }

        // DELETE: api/ClienteDistribuidors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClienteDistribuidor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteDistribuidor = await _context.ClienteDistribuidor.FindAsync(id);
            if (clienteDistribuidor == null)
            {
                return NotFound();
            }

            _context.ClienteDistribuidor.Remove(clienteDistribuidor);
            await _context.SaveChangesAsync();

            return Ok(clienteDistribuidor);
        }

        private bool ClienteDistribuidorExists(int id)
        {
            return _context.ClienteDistribuidor.Any(e => e.ClienteId == id);
        }
    }
}