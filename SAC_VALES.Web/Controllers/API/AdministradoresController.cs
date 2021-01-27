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
    public class AdministradoresController : ControllerBase
    {
        private readonly DataContext _context;

        public AdministradoresController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Administradores
        [HttpGet]
        public IEnumerable<AdministradorEntity> GetAdministrador()
        {
            return _context.Administrador;
        }

        // GET: api/Administradores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdministradorEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administradorEntity = await _context.Administrador.FindAsync(id);

            if (administradorEntity == null)
            {
                return NotFound();
            }

            return Ok(administradorEntity);
        }

        // PUT: api/Administradores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministradorEntity([FromRoute] int id, [FromBody] AdministradorEntity administradorEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != administradorEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(administradorEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministradorEntityExists(id))
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

        // POST: api/Administradores
        [HttpPost]
        public async Task<IActionResult> PostAdministradorEntity([FromBody] AdministradorEntity administradorEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Administrador.Add(administradorEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdministradorEntity", new { id = administradorEntity.id }, administradorEntity);
        }

        // DELETE: api/Administradores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministradorEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administradorEntity = await _context.Administrador.FindAsync(id);
            if (administradorEntity == null)
            {
                return NotFound();
            }

            _context.Administrador.Remove(administradorEntity);
            await _context.SaveChangesAsync();

            return Ok(administradorEntity);
        }

        private bool AdministradorEntityExists(int id)
        {
            return _context.Administrador.Any(e => e.id == id);
        }
    }
}