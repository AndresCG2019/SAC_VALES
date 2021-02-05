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
using SAC_VALES.Web.Helpers;

namespace SAC_VALES.Web.Controllers
{
    [Authorize(Roles = "Distribuidor")]
    public class ClienteEntitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ClienteEntitiesController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: ClienteEntities
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            return View(await _context.Cliente
               .Where(c =>c.Distribuidor.Id == user.Id )
               .ToListAsync());
        }

        // GET: ClienteEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteEntity = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (clienteEntity == null)
            {
                return NotFound();
            }

            return View(clienteEntity);
        }

        // GET: ClienteEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClienteEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,status_cliente")] ClienteEntity clienteEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clienteEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clienteEntity);
        }

        // GET: ClienteEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteEntity = await _context.Cliente.FindAsync(id);
            if (clienteEntity == null)
            {
                return NotFound();
            }
            return View(clienteEntity);
        }

        // POST: ClienteEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,status_cliente,Nombre,Direccion,Email,Apellidos,Telefono")]
        ClienteEntity clienteEntity)
        {
            if (id != clienteEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clienteEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteEntityExists(clienteEntity.id))
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
            return View(clienteEntity);
        }

        // GET: ClienteEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteEntity = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (clienteEntity == null)
            {
                return NotFound();
            }

            return View(clienteEntity);
        }

        // POST: ClienteEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clienteEntity = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(clienteEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteEntityExists(int id)
        {
            return _context.Cliente.Any(e => e.id == id);
        }
    }
}
