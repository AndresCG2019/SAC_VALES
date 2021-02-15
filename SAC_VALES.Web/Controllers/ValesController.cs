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
using SAC_VALES.Web.Helpers;

namespace SAC_VALES.Web.Controllers
{
    [Authorize(Roles = "Distribuidor")]
    public class ValesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ValesController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: Vales
        public async Task<IActionResult> Index()
        {
            DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            return View(await _context.Vale
                .Where(v => v.DistribuidorId == distribuidor.id && v.status_vale == true)
                .ToListAsync());
        }

        // GET: Vales/Create
        public IActionResult Create()
        {
            DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Cliente_id = new SelectList(_context.Cliente.ToList(),"id", "Email" );
            ViewBag.Empresa_id = new SelectList(_context.Empresa, "id", "Email");

            return View();
        }

        // POST: Vales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Monto,ClienteId,EmpresaId,DistribuidorId,Fecha,status_vale")] ValeEntity valeEntity)
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            DistribuidorEntity distribuidor = _context.Distribuidor
              .Where(d => d.Email == user.Email).FirstOrDefault();

            if (ModelState.IsValid)
            {
                valeEntity.DistribuidorId = distribuidor.id;
                valeEntity.Fecha = DateTime.UtcNow;
                valeEntity.status_vale = true;

                _context.Add(valeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cliente_id = new SelectList(_context.Cliente.ToList(), "id", "Email");
            ViewBag.Empresa_id = new SelectList(_context.Empresa, "id", "Email");

            return View(valeEntity);
        }

        // GET: Vales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valeEntity = await _context.Vale.FindAsync(id);

            ClienteEntity cliente = await _context.Cliente.Where(c => c.id == valeEntity.ClienteId).FirstOrDefaultAsync();

            ViewBag.ClienteEmail = cliente.Email;
            ViewBag.ClienteNombre = cliente.Nombre;
            ViewBag.ClienteApellidos = cliente.Apellidos;

            if (valeEntity == null)
            {
                return NotFound();
            }
            return View(valeEntity);
        }

        // POST: Vales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Monto,DistribuidorId,EmpresaId,ClienteId,status_vale")] ValeEntity valeEntity)
        {
            if (id != valeEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(valeEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValeEntityExists(valeEntity.id))
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
            return View(valeEntity);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vale = await _context.Vale.FindAsync(id);
            if (vale == null)
            {
                return NotFound();
            }
            return View(vale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id, [Bind("id,Monto,DistribuidorId,EmpresaId,ClienteId,status_vale")]
        ValeEntity valeEntity)
        {

            if (id != valeEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    valeEntity.status_vale = false;

                    _context.Update(valeEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValeEntityExists(valeEntity.id))
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
            return View(valeEntity);
        }

        private bool ValeEntityExists(int id)
        {
            return _context.Vale.Any(e => e.id == id);
        }
    }
}
