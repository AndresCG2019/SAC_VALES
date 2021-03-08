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
    public class DistribuidorEntitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public DistribuidorEntitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DistribuidorEntities
        [HttpGet]
        public IEnumerable<DistribuidorEntity> GetDistribuidor()
        {
            return _context.Distribuidor;
        }

        // GET: api/DistribuidorEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistribuidorEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distribuidorEntity = await _context.Distribuidor.FindAsync(id);

            if (distribuidorEntity == null)
            {
                return NotFound();
            }

            return Ok(distribuidorEntity);
        }

        // PUT: api/DistribuidorEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistribuidorEntity([FromRoute] int id, [FromBody] DistribuidorEntity distribuidorEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != distribuidorEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(distribuidorEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistribuidorEntityExists(id))
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

        // POST: api/DistribuidorEntities
        [HttpPost]
        public async Task<IActionResult> PostDistribuidorEntity([FromBody] DistribuidorEntity distribuidorEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Distribuidor.Add(distribuidorEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistribuidorEntity", new { id = distribuidorEntity.id }, distribuidorEntity);
        }

        // DELETE: api/DistribuidorEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistribuidorEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distribuidorEntity = await _context.Distribuidor.FindAsync(id);
            if (distribuidorEntity == null)
            {
                return NotFound();
            }

            _context.Distribuidor.Remove(distribuidorEntity);
            await _context.SaveChangesAsync();

            return Ok(distribuidorEntity);
        }

        private bool DistribuidorEntityExists(int id)
        {
            return _context.Distribuidor.Any(e => e.id == id);
        }
    }
}