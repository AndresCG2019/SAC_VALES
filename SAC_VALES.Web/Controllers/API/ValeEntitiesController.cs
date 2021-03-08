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
    public class ValeEntitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValeEntitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ValeEntities
        [HttpGet]
        public IEnumerable<ValeEntity> GetVale()
        {
            return _context.Vale;
        }

        // GET: api/ValeEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValeEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var valeEntity = await _context.Vale.FindAsync(id);

            if (valeEntity == null)
            {
                return NotFound();
            }

            return Ok(valeEntity);
        }

        // PUT: api/ValeEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValeEntity([FromRoute] int id, [FromBody] ValeEntity valeEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != valeEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(valeEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValeEntityExists(id))
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

        // POST: api/ValeEntities
        [HttpPost]
        public async Task<IActionResult> PostValeEntity([FromBody] ValeEntity valeEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Vale.Add(valeEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValeEntity", new { id = valeEntity.id }, valeEntity);
        }

        // DELETE: api/ValeEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValeEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var valeEntity = await _context.Vale.FindAsync(id);
            if (valeEntity == null)
            {
                return NotFound();
            }

            _context.Vale.Remove(valeEntity);
            await _context.SaveChangesAsync();

            return Ok(valeEntity);
        }

        private bool ValeEntityExists(int id)
        {
            return _context.Vale.Any(e => e.id == id);
        }
    }
}