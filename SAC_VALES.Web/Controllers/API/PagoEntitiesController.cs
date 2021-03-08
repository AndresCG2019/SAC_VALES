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
    public class PagoEntitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public PagoEntitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PagoEntities
        [HttpGet]
        public IEnumerable<PagoEntity> GetPago()
        {
            return _context.Pago;
        }

        // GET: api/PagoEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pagoEntity = await _context.Pago.FindAsync(id);

            if (pagoEntity == null)
            {
                return NotFound();
            }

            return Ok(pagoEntity);
        }

        // PUT: api/PagoEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagoEntity([FromRoute] int id, [FromBody] PagoEntity pagoEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pagoEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(pagoEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoEntityExists(id))
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

        // POST: api/PagoEntities
        [HttpPost]
        public async Task<IActionResult> PostPagoEntity([FromBody] PagoEntity pagoEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pago.Add(pagoEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPagoEntity", new { id = pagoEntity.id }, pagoEntity);
        }

        // DELETE: api/PagoEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagoEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pagoEntity = await _context.Pago.FindAsync(id);
            if (pagoEntity == null)
            {
                return NotFound();
            }

            _context.Pago.Remove(pagoEntity);
            await _context.SaveChangesAsync();

            return Ok(pagoEntity);
        }

        private bool PagoEntityExists(int id)
        {
            return _context.Pago.Any(e => e.id == id);
        }
    }
}