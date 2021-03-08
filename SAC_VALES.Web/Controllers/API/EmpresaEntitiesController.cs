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
    public class EmpresaEntitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public EmpresaEntitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/EmpresaEntities
        [HttpGet]
        public IEnumerable<EmpresaEntity> GetEmpresa()
        {
            return _context.Empresa;
        }

        // GET: api/EmpresaEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpresaEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var empresaEntity = await _context.Empresa.FindAsync(id);

            if (empresaEntity == null)
            {
                return NotFound();
            }

            return Ok(empresaEntity);
        }

        // PUT: api/EmpresaEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresaEntity([FromRoute] int id, [FromBody] EmpresaEntity empresaEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresaEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(empresaEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaEntityExists(id))
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

        // POST: api/EmpresaEntities
        [HttpPost]
        public async Task<IActionResult> PostEmpresaEntity([FromBody] EmpresaEntity empresaEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Empresa.Add(empresaEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpresaEntity", new { id = empresaEntity.id }, empresaEntity);
        }

        // DELETE: api/EmpresaEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresaEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var empresaEntity = await _context.Empresa.FindAsync(id);
            if (empresaEntity == null)
            {
                return NotFound();
            }

            _context.Empresa.Remove(empresaEntity);
            await _context.SaveChangesAsync();

            return Ok(empresaEntity);
        }

        private bool EmpresaEntityExists(int id)
        {
            return _context.Empresa.Any(e => e.id == id);
        }
    }
}