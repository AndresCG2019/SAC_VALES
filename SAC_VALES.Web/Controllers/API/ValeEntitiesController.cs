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
        public async Task<IActionResult> CancelarVale([FromRoute] int id)
        {
            ValeEntity vale = _context.Vale.Where(v => v.id == id).FirstOrDefault();

            vale.status_vale = "Inactivo";

            try
            {
                _context.Update(vale);
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

        [HttpPost]
        [Route("PostVale")]
        public async Task<IActionResult> PostVale([FromBody] CreateValeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            request.FechaCreacion = DateTime.UtcNow;

            EmpresaEntity empresa = _context.Empresa.Where(e => e.id == request.EmpresaId).FirstOrDefault();

            if (empresa == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "La empresa especificada no existe."
                });
            }

            ClienteEntity cliente = _context.Cliente.Where(c => c.id == request.ClienteId).FirstOrDefault();

            if (cliente == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El cliente especificado no existe."
                });
            }

            DistribuidorEntity dist = _context.Distribuidor.Where(d => d.id == request.DistId).FirstOrDefault();

            if (dist == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El distribuidor especificado no existe."
                });
            }

            TaloneraEntity talonera = _context.Talonera.Where(t => t.id == request.TaloneraId).FirstOrDefault();

            if (talonera == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "La talonera especificada no existe."
                });
            }

            // evalua que el folio del vale ingresado este dentro del rango de la talonera a la que pertenece

            if (talonera == null ||
                request.NumeroFolio < talonera.RangoInicio || request.NumeroFolio > talonera.RangoFin)
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Folio fuera de rango."
                });

            ValeEntity valeValidacion = _context.Vale
                .Where(v => v.Talonera.id == talonera.id && v.NumeroFolio == request.NumeroFolio)
                .FirstOrDefault();

            // evalua que el folio del vale ingresado no se repita dentro de la talonera a la que pertenece

            if (valeValidacion != null)
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Folio repetido."
                });

            ValeEntity valeInsert = new ValeEntity
            {
                Monto = request.Monto,
                CantidadPagos = request.CantidadPagos,
                NumeroFolio = request.NumeroFolio,
                FechaCreacion = request.FechaCreacion,
                FechaPrimerPago = request.FechaPrimerPago,
                Distribuidor = dist,
                Cliente = cliente,
                Empresa = empresa,
                Talonera = talonera,
                status_vale = "Activo"

            };

            _context.Vale.Add(valeInsert);

            await _context.SaveChangesAsync();

            float division = valeInsert.Monto / valeInsert.CantidadPagos;
            DateTime Fecha = valeInsert.FechaPrimerPago;


            for (int i = 0; i < valeInsert.CantidadPagos; i++)
            {

                _context.Pago.Add(new PagoEntity
                {
                    Cantidad = division,
                    FechaLimite = Fecha,
                    Vale = valeInsert

                });
                //ENERO
                if (Fecha.Month == 1 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 1 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 1 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }

                //FEBRERO
                else if (Fecha.Month == 2 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 2 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 2 && Fecha.Day == 28)
                {
                    Fecha = Fecha.AddDays(15);
                }

                //MARZO
                else if (Fecha.Month == 3 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 3 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 3 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }

                //ABRIL
                else if (Fecha.Month == 4 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 4 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 4 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(15);
                }

                //MAYO
                else if (Fecha.Month == 5 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 5 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 5 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }

                //JUNIO
                else if (Fecha.Month == 6 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 6 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 6 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(15);
                }

                //JULIO
                else if (Fecha.Month == 7 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 7 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 7 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }

                //AGOSTO
                else if (Fecha.Month == 8 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 8 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 8 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }

                //SEPTIEMBRE
                else if (Fecha.Month == 9 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 9 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 9 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(15);
                }

                //OCTUBRE
                else if (Fecha.Month == 10 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 10 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 10 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }

                //NOVIEMBRE
                else if (Fecha.Month == 11 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 11 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 11 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(15);
                }
                //DICIEMBRE
                else if (Fecha.Month == 12 && Fecha.Day == 1)
                {
                    Fecha = Fecha.AddDays(14);
                }
                else if (Fecha.Month == 12 && Fecha.Day == 15)
                {
                    Fecha = Fecha.AddDays(15);
                }
                else if (Fecha.Month == 12 && Fecha.Day == 30)
                {
                    Fecha = Fecha.AddDays(16);
                }
                //Fecha = Fecha.AddDays(15);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ValeEntityExists(int id)
        {
            return _context.Vale.Any(e => e.id == id);
        }
    }
}