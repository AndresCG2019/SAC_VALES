using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Controllers
{
    [Authorize(Roles = "Distribuidor")]
    public class TalonerasController : Controller
    {
        private readonly DataContext _context;

        public TalonerasController(DataContext context)
        {
            _context = context;
        }

        // GET: Taloneras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Talonera
                .Include(item => item.Empresa)
                .Where(t => t.Distribuidor.Email == User.Identity.Name && t.StatusTalonera == "Activo")
                .ToListAsync());
        }

        public IActionResult ErrorTalonera()
        {
            return View();
        }

        public async Task<IActionResult> ValesTalonera(int? id)
        {
            if (id == null) return NotFound();

            return View(await _context.Vale.Where(v => v.Talonera.id == id && v.status_vale == "Activo").ToListAsync());
        }


        // GET: Taloneras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taloneraEntity = await _context.Talonera
                .Include(item => item.Empresa)
                .FirstOrDefaultAsync(m => m.id == id);
            if (taloneraEntity == null)
            {
                return NotFound();
            }

            return View(taloneraEntity);
        }

        public async Task<IActionResult> SelectEmpresa()
        {
            
            return View(await _context.Empresa.ToListAsync());
        }

        // GET: Taloneras/Create
        public IActionResult Create(int? id)
        {
            if (id == null) return NotFound();

            EmpresaEntity empresa = _context.Empresa.Where(e => e.id == id).FirstOrDefault();

            ViewBag.emailEmpresa = empresa.Email;

            return View();
        }

        // POST: Taloneras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,RangoInicio,RangoFin,Empresa")] 
        TaloneraEntity taloneraEntity, int? id)
        {
            if (ModelState.IsValid)
            {
                // valida que el inicio de rango sea menor que el final de rango
                if (taloneraEntity.RangoInicio >= taloneraEntity.RangoFin
                        || taloneraEntity.RangoFin <= taloneraEntity.RangoInicio
                    )
                    return RedirectToAction(nameof(ErrorTalonera));

                EmpresaEntity empresa = _context.Empresa.Where(e => e.id == id).FirstOrDefault();

                DistribuidorEntity distribuidor = _context.Distribuidor
                    .Where(d => d.Email == User.Identity.Name).FirstOrDefault();

                _context.Talonera.Add(new TaloneraEntity
                {
                    RangoInicio = taloneraEntity.RangoInicio,
                    RangoFin = taloneraEntity.RangoFin,
                    Empresa = empresa,
                    Distribuidor = distribuidor,
                    StatusTalonera = "Activo"
                });;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(taloneraEntity);
        }

        // GET: Taloneras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taloneraEntity = await _context.Talonera.FindAsync(id);
            if (taloneraEntity == null)
            {
                return NotFound();
            }
            return View(taloneraEntity);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talonera = await _context.Talonera.FindAsync(id);
            if (talonera == null)
            {
                return NotFound();
            }
            return View(talonera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id,
           [Bind("id, RangoInicio, RangoFin")] TaloneraEntity talonera)
        {

            if (id != talonera.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    talonera.StatusTalonera = "Inactivo";

                    _context.Update(talonera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaloneraEntityExists(talonera.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(talonera);
        }


        // POST: Taloneras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,RangoInicio,RangoFin")] TaloneraEntity taloneraEntity)
        {
            if (id != taloneraEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taloneraEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaloneraEntityExists(taloneraEntity.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taloneraEntity);
        }

        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> EdoCuenta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taloneraEntity = await _context.Talonera
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.id == id);
            if (taloneraEntity == null)
            {
                return NotFound();
            }

            List<ValeEntity> vales = await _context.Vale
                .Where(v => v.Talonera.id == id && v.status_vale == "Activo" && v.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosCompletos = await _context.Pago
                .Where(p => p.Vale.Talonera.id == id && p.Pagado == true
                    && p.Vale.status_vale == "Activo" && p.Vale.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosPendientes = await _context.Pago
                .Where(p => p.Vale.Talonera.id == id && p.Pagado == false
                    && p.Vale.status_vale == "Activo" && p.Vale.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            float montoTotal = 0;
            float montoPendiente = 0;
            float montoPagado = 0;

            for (int i = 0; i < vales.Count; i++)
            {
                Debug.WriteLine("MONTO");
                Debug.WriteLine(vales[i].Monto);

                montoTotal = montoTotal + vales[i].Monto;
            }

            for (int i = 0; i < pagosCompletos.Count; i++)
            {
                Debug.WriteLine("MONTO PAGADO");
                Debug.WriteLine(pagosCompletos[i].Cantidad);

                montoPagado = montoPagado + pagosCompletos[i].Cantidad;
            }

            for (int i = 0; i < pagosPendientes.Count; i++)
            {
                Debug.WriteLine("MONTO PENDIENTE");
                Debug.WriteLine(pagosPendientes[i].Cantidad);

                montoPendiente = montoPendiente + pagosPendientes[i].Cantidad;
            }

            ViewBag.MontoTotal = montoTotal;
            ViewBag.MontoPendiente = montoPendiente;
            ViewBag.MontoPagado = montoPagado;

            return View(taloneraEntity);
        }
        private bool TaloneraEntityExists(int id)
        {
            return _context.Talonera.Any(e => e.id == id);
        }
    }
}
