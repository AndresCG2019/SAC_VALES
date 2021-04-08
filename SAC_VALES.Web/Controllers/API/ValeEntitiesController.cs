using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;

namespace SAC_VALES.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValeEntitiesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public ValeEntitiesController(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        [Route("GetValesByDist")]
        public async Task<IActionResult> GetValesByDist([FromBody] DistValesRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dist = _context.Distribuidor.Where(d => d.id == request.DistId).FirstOrDefault();

            if (dist == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El distribuidor especificado no existe."
                });
            }

            var vales = await _context.Vale
                .Include(v => v.Distribuidor)
                .Include(v => v.Empresa)
                .Include(v => v.Cliente)
                .Include(v => v.Talonera)
                .Include(v => v.Talonera.Empresa)
                .Where(v => v.Distribuidor.id == request.DistId && v.status_vale == "Activo").ToListAsync();

            var pagos = await _context.Pago
                .Include(p => p.Vale)
                .Where(p => p.Vale.Distribuidor.id == request.DistId)
                .ToListAsync();

            return Ok(_converterHelper.ToValesResponse(vales, pagos));
        }

        [HttpPost]
        [Route("GetValesByClie")]
        public async Task<IActionResult> GetValesByClie([FromBody] ClieValesRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var clie = _context.Cliente.Where(d => d.id == request.ClieId).FirstOrDefault();

            if (clie == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El cliente especificado no existe."
                });
            }

            var vales = await _context.Vale
                .Include(v => v.Distribuidor)
                .Include(v => v.Empresa)
                .Include(v => v.Cliente)
                .Include(v => v.Talonera)
                .Include(v => v.Talonera.Empresa)
                .Where(v => v.Cliente.id == request.ClieId && v.status_vale == "Activo").ToListAsync();

            var pagos = await _context.Pago
                .Include(p => p.Vale)
                .Where(p => p.Vale.Cliente.id == request.ClieId)
                .ToListAsync();

            return Ok(_converterHelper.ToValesResponse(vales, pagos));
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