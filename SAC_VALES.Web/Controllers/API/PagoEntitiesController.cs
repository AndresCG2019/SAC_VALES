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
    public class PagoEntitiesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public PagoEntitiesController(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
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
        public async Task<IActionResult> MarcarPago([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PagoEntity pago = _context.Pago.Where(p => p.id == id).FirstOrDefault();

            if (pago.Pagado == true)
            {
                pago.Pagado = false;
            }
            else
            {
                pago.Pagado = true;
            }

            try
            {
                _context.Update(pago);
                await _context.SaveChangesAsync();

                ValeEntity vale = _context.Vale.Where(v => v.id == pago.Valeid).FirstOrDefault();

                List<PagoEntity> pagos = await _context.Pago
                    .Where(p => p.Valeid == pago.Valeid && p.Pagado == true)
                    .ToListAsync();

                if (pagos.Count == vale.CantidadPagos)
                    vale.Pagado = true;
                else
                    vale.Pagado = false;

                _context.Update(vale);
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

        [HttpPost]
        [Route("GetPagosByVale")]
        public async Task<IActionResult> GetPagosByVale([FromBody] PagosByValeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var vale = _context.Vale.Where(v => v.id == request.ValeId).FirstOrDefault();

            if (vale == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El vale especificado no existe."
                });
            }

            var pagos = await _context.Pago
                .Include(p => p.Vale)
                .Where(p => p.Vale.id == vale.id)
                .ToListAsync();

            return Ok(_converterHelper.ToPagosResponse(pagos, vale.id));
        }


        private bool PagoEntityExists(int id)
        {
            return _context.Pago.Any(e => e.id == id);
        }
    }
}