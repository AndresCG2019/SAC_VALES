using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;

namespace SAC_VALES.Web.Controllers
{
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
            return View(await _context.Vale.ToListAsync());
        }

        // GET: Vales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valeEntity = await _context.Vale
                .FirstOrDefaultAsync(m => m.id == id);
            if (valeEntity == null)
            {
                return NotFound();
            }

            return View(valeEntity);
        }

        // GET: Vales/Create
        public IActionResult Create()
        {
            DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Cliente_id = new SelectList(_context.Cliente.Where(c => c.Distribuidor.Email == User.Identity.Name).ToList(),"id", "Email" );
            ViewBag.Empresa_id = new SelectList(_context.Empresa, "id", "Email");

            return View();
        }

        // POST: Vales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Monto,ClienteId,EmpresaId,DistribuidorId")] ValeEntity valeEntity)
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            DistribuidorEntity distribuidor = _context.Distribuidor
              .Where(d => d.Email == user.Email).FirstOrDefault();

            if (ModelState.IsValid)
            {
                valeEntity.DistribuidorId = distribuidor.id;

                _context.Add(valeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cliente_id = new SelectList(_context.Cliente.Where(c => c.Distribuidor.Email == User.Identity.Name).ToList(), "id", "Email");
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
        public async Task<IActionResult> Edit(int id, [Bind("id,Monto")] ValeEntity valeEntity)
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

        // GET: Vales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valeEntity = await _context.Vale
                .FirstOrDefaultAsync(m => m.id == id);
            if (valeEntity == null)
            {
                return NotFound();
            }

            return View(valeEntity);
        }

        // POST: Vales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var valeEntity = await _context.Vale.FindAsync(id);
            _context.Vale.Remove(valeEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValeEntityExists(int id)
        {
            return _context.Vale.Any(e => e.id == id);
        }
    }
}
