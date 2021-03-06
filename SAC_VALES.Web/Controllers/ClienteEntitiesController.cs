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

  //  [Authorize(Roles = "Distribuidor")]
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
            DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            return View(await _context.ClienteDistribuidor
                .Include(item => item.Cliente)
                .Where(cd => cd.DistribuidorId == distribuidor.id)
                .ToListAsync());
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> EdoCuentaCliente(int? id)
        {
            if (id == null)
            {
                Debug.WriteLine("Entre aqui 1");
                return NotFound();
            }

            var distribuidorEntity = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (distribuidorEntity == null)
            {
                Debug.WriteLine("Entre aqui 2");
                return NotFound();
            }

            List<ValeEntity> vales = await _context.Vale
                .Where(v => v.Distribuidor.id == id && v.status_vale == "Activo" && v.Cliente.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosCompletos = await _context.Pago
                .Where(p => p.Vale.Distribuidor.id == id && p.Pagado == true
                    && p.Vale.status_vale == "Activo" && p.Vale.Cliente.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosPendientes = await _context.Pago
                .Where(p => p.Vale.Distribuidor.id == id && p.Pagado == false
                    && p.Vale.status_vale == "Activo" && p.Vale.Cliente.Email == User.Identity.Name)
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

            return View(distribuidorEntity);
        }

        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> EdoCuentaClienteDis(int? id)
        {
            if (id == null)
            {
                Debug.WriteLine("Entre aqui 1");
                return NotFound();
            }

            var distribuidorEntity = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (distribuidorEntity == null)
            {
                Debug.WriteLine("Entre aqui 2");
                return NotFound();
            }

            List<ValeEntity> vales = await _context.Vale
                .Where(v => v.Cliente.id == id && v.status_vale == "Activo" && v.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosCompletos = await _context.Pago
                .Where(p => p.Vale.Cliente.id == id && p.Pagado == true
                    && p.Vale.status_vale == "Activo" && p.Vale.Distribuidor.Email == User.Identity.Name)
                .ToListAsync();

            List<PagoEntity> pagosPendientes = await _context.Pago
                .Where(p => p.Vale.Cliente.id == id && p.Pagado == false
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

            return View(distribuidorEntity);
        }
        public async Task<IActionResult> SearchCliente()
        {
            return View();
        }

        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View( await _context.Cliente.Where(c => c.Email.Contains(SearchPhrase)).ToListAsync());
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
            //return View(clienteEntity); TEMPORALMENTE INHABILITADO
            return NotFound();
        }

        public async Task<IActionResult> Vincular(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vincular(int id, [Bind("id,Email,Nombre,Apellidos,Direccion,Telefono")]
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
                    DistribuidorEntity distribuidor = _context.Distribuidor
                                .Where(d => d.Email == User.Identity.Name).FirstOrDefault();

                    _context.ClienteDistribuidor.Add(new ClienteDistribuidor
                    {
                        ClienteId = clienteEntity.id,
                        DistribuidorId = distribuidor.id,
                    });

                    await _context.SaveChangesAsync();


                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return RedirectToAction(nameof(Index));
                }
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
