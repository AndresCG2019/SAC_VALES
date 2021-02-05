using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Common.Enums;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministradoresController : Controller
    {
        
        private readonly DataContext _context;

        public AdministradoresController(DataContext context)
        {
            _context = context;
        }

        // GET: Administradores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administrador.ToListAsync());
        }
        // GET: Administradores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administradorEntity = await _context.Administrador
                .FirstOrDefaultAsync(m => m.id == id);
            if (administradorEntity == null)
            {
                return NotFound();
            }

            return View(administradorEntity);
        }

        // GET: Administradores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administradores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Nombre, ApellidoP, ApellidoM,Telefono")] AdministradorEntity administradorEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administradorEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administradorEntity);
        }

        // GET: Administradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administradorEntity = await _context.Administrador.FindAsync(id);
            if (administradorEntity == null)
            {
                return NotFound();
            }
            return View(administradorEntity);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Nombre")] AdministradorEntity administradorEntity)
        {
            if (id != administradorEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administradorEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministradorEntityExists(administradorEntity.id))
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
            return View(administradorEntity);
        }

        // GET: Administradores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administradorEntity = await _context.Administrador
                .FirstOrDefaultAsync(m => m.id == id);
            if (administradorEntity == null)
            {
                return NotFound();
            }

            return View(administradorEntity);
        }

        // POST: Administradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administradorEntity = await _context.Administrador.FindAsync(id);
            _context.Administrador.Remove(administradorEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministradorEntityExists(int id)
        {
            return _context.Administrador.Any(e => e.id == id);
        }
    }
}
