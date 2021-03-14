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
    [Authorize(Roles = "Admin,Distribuidor")]
    public class EmpresasController : Controller
    {
        private readonly DataContext _context;

        public EmpresasController(DataContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresa.ToListAsync());
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaEntity = await _context.Empresa
                .FirstOrDefaultAsync(m => m.id == id);
            if (empresaEntity == null)
            {
                return NotFound();
            }

            return View(empresaEntity);
        }

        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> EdoCuenta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaEntity = await _context.Empresa
                .FirstOrDefaultAsync(m => m.id == id);
            if (empresaEntity == null)
            {
                return NotFound();
            }

            List <ValeEntity> vales = await _context.Vale
                .Where(v => v.Empresa.id == id && v.status_vale == "Activo" && v.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosCompletos = await _context.Pago
                .Where(p => p.Vale.Empresa.id == id && p.Pagado == true
                    && p.Vale.status_vale == "Activo" && p.Vale.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosPendientes = await _context.Pago
                .Where(p => p.Vale.Empresa.id == id && p.Pagado == false 
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

            for (int i = 0; i < pagosPendientes.Count; i ++) 
            {
                Debug.WriteLine("MONTO PENDIENTE");
                Debug.WriteLine(pagosPendientes[i].Cantidad);

                montoPendiente = montoPendiente + pagosPendientes[i].Cantidad;
            }

            ViewBag.MontoTotal = montoTotal;
            ViewBag.MontoPendiente = montoPendiente;
            ViewBag.MontoPagado = montoPagado;

            return View(empresaEntity);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NombreEmpresa,NombreRepresentante,ApellidosRepresentante,TelefonoRepresentante,Email,Direccion")] EmpresaEntity empresaEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresaEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresaEntity);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaEntity = await _context.Empresa.FindAsync(id);
            if (empresaEntity == null)
            {
                return NotFound();
            }
            return View(empresaEntity);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NombreEmpresa,NombreRepresentante,ApellidosRepresentante,TelefonoRepresentante,Email,Direccion")] EmpresaEntity empresaEntity)
        {
            if (id != empresaEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresaEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaEntityExists(empresaEntity.id))
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
            return View(empresaEntity);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaEntity = await _context.Empresa
                .FirstOrDefaultAsync(m => m.id == id);
            if (empresaEntity == null)
            {
                return NotFound();
            }

            return View(empresaEntity);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresaEntity = await _context.Empresa.FindAsync(id);
            _context.Empresa.Remove(empresaEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaEntityExists(int id)
        {
            return _context.Empresa.Any(e => e.id == id);
        }
    }
}
