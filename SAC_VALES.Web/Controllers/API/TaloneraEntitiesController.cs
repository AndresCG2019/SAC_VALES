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
    public class TaloneraEntitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public TaloneraEntitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TaloneraEntities
        [HttpGet]
        public IEnumerable<TaloneraEntity> GetTalonera()
        {
            return _context.Talonera;
        }

        // GET: api/TaloneraEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaloneraEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taloneraEntity = await _context.Talonera.FindAsync(id);

            if (taloneraEntity == null)
            {
                return NotFound();
            }

            return Ok(taloneraEntity);
        }

        // PUT: api/TaloneraEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaloneraEntity([FromRoute] int id, [FromBody] TaloneraEntity taloneraEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taloneraEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(taloneraEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaloneraEntityExists(id))
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

        // POST: api/TaloneraEntities
        [HttpPost]
        public async Task<IActionResult> PostTaloneraEntity([FromBody] TaloneraEntity taloneraEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Talonera.Add(taloneraEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaloneraEntity", new { id = taloneraEntity.id }, taloneraEntity);
        }

        // DELETE: api/TaloneraEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaloneraEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taloneraEntity = await _context.Talonera.FindAsync(id);
            if (taloneraEntity == null)
            {
                return NotFound();
            }

            _context.Talonera.Remove(taloneraEntity);
            await _context.SaveChangesAsync();

            return Ok(taloneraEntity);
        }

        private bool TaloneraEntityExists(int id)
        {
            return _context.Talonera.Any(e => e.id == id);
        }
    }
}