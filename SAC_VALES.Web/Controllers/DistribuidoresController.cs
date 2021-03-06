﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Common.Enums;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;

namespace SAC_VALES.Web.Controllers
{
    public class DistribuidoresController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public DistribuidoresController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: Distribuidores
        [Authorize(Roles = "Admin,Empresa")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Distribuidor.ToListAsync());
        }

        // GET: Distribuidores
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ClientSideIndex()
        {
            ClienteEntity cliente = _context.Cliente.Where(c => c.Email == User.Identity.Name).FirstOrDefault();
            
            return View(await _context.ClienteDistribuidor
               .Include(item => item.Distribuidor)
               .Where(cd => cd.ClienteId == cliente.id)
               .ToListAsync());
        }

        // GET: Distribuidores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuidorEntity = await _context.Distribuidor
                .FirstOrDefaultAsync(m => m.id == id);
            if (distribuidorEntity == null)
            {
                return NotFound();
            }

            return View(distribuidorEntity);
        }

        // GET: Distribuidores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distribuidores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,EmpresaVinculada,StatusDistribuidor")] DistribuidorEntity distribuidorEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distribuidorEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distribuidorEntity);
        }

        // GET: Distribuidores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuidorEntity = await _context.Distribuidor.FindAsync(id);
            if (distribuidorEntity == null)
            {
                return NotFound();
            }
            //return View(distribuidorEntity); DESHABILITADO TEMPORALMENTE
            return NotFound();
        }

        // POST: Distribuidores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmpresaVinculada,StatusDistribuidor,Nombre,Apellidos,Direccion,Telefono,Email")]
        DistribuidorEntity distribuidorEntity)
        {
            if (id != distribuidorEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distribuidorEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistribuidorEntityExists(distribuidorEntity.id))
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
            return View(distribuidorEntity);
        }

        // GET: Distribuidores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuidorEntity = await _context.Distribuidor
                .FirstOrDefaultAsync(m => m.id == id);
            if (distribuidorEntity == null)
            {
                return NotFound();
            }

            return View(distribuidorEntity);
        }

        // POST: Distribuidores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distribuidorEntity = await _context.Distribuidor.FindAsync(id);
            _context.Distribuidor.Remove(distribuidorEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistribuidorEntityExists(int id)
        {
            return _context.Distribuidor.Any(e => e.id == id);
        }
    }
}
