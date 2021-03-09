﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Controllers
{
    [Authorize(Roles = "Distribuidor,Admin, Cliente")]
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
        public async Task<IActionResult> Index(int? id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Distribuidor"))
            {
                DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

                return View(await _context.Vale
                    .Include(i => i.Empresa)
                    .Include(i => i.Cliente)
                    .Include(i => i.Talonera)
                    .Where(v => v.Distribuidor.id == distribuidor.id && v.status_vale == "Activo")
                    .ToListAsync());
            }
            else 
            {
                if (id == null)
                    return NotFound();

                DistribuidorEntity dist = _context.Distribuidor.Where(d => d.id == id).FirstOrDefault();
                ViewBag.EmailDist = dist.Email;

                return View(await _context.Vale
                   .Include(i => i.Empresa)
                   .Include(i => i.Cliente)
                   .Include(i => i.Talonera)
                   .Where(v => v.Distribuidor.id == id && v.status_vale == "Activo")
                   .ToListAsync());
            }
        }

        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> MarcarPagado(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        [Authorize(Roles = "Distribuidor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarPagado(int id,
          [Bind("id,Cantidad,FechaLimite,Pagado,Valeid")]
        PagoEntity pagoEntity)
        {

            if (id != pagoEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {   
                    if (pagoEntity.Pagado == false)
                        pagoEntity.Pagado = true;
                    else
                        pagoEntity.Pagado = false;

                    _context.Update(pagoEntity);
                    await _context.SaveChangesAsync();

                    ValeEntity vale = _context.Vale.Where(v => v.id == pagoEntity.Valeid).FirstOrDefault();

                    List<PagoEntity> pagos = await _context.Pago
                        .Where(p => p.Valeid == pagoEntity.Valeid && p.Pagado == true)
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
                    if (!ValeEntityExists(pagoEntity.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("VerPagos/" + pagoEntity.Valeid);
            }
            return View(pagoEntity);
        }

        public async Task<IActionResult> VerPagos(int? id)
        {
            ValeEntity vale = _context.Vale.Where(v => v.id == id).FirstOrDefault();

            if (vale.Pagado == true)
                ViewBag.Pagado = true;
            else
                ViewBag.Pagado = false;

            return View(await _context.Pago.Where(p => p.Vale.id == id).ToListAsync());
        }

        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> SelectTalonera()
        {
            return View(await _context.Talonera
                .Where(t => t.Distribuidor.Email == User.Identity.Name)
                .Include(i => i.Empresa)
                .ToListAsync());
        }

        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> SelectCliente(int? id)
        {
            DistribuidorEntity distribuidor = _context.Distribuidor
                .Where(d => d.Email == User.Identity.Name)
                .FirstOrDefault();

            ViewBag.idTalonera = id;

            return View(await _context.ClienteDistribuidor
                .Include(item => item.Cliente)
                .Where(cd => cd.DistribuidorId == distribuidor.id)
                .ToListAsync());
        }

        public IActionResult ErrorVale()
        {
            return View();
        }

        // GET: Vales/Create
        [Authorize(Roles = "Distribuidor")]
        public IActionResult Create(int? idTalonera, int? idCliente)
        {
            ViewBag.idTalonera = idTalonera;
            ViewBag.idCliente = idCliente;

            if (idTalonera == null) return NotFound();

            TaloneraEntity talonera = _context.Talonera
                .Include(item => item.Empresa)
                .Where(t => t.id == idTalonera)
                .FirstOrDefault();

            ViewBag.Talonera = talonera;

            ClienteEntity cliente = _context.Cliente
                .Where(c => c.id == idCliente)
                .FirstOrDefault();

            ViewBag.Cliente = cliente;

            // generacion de numero de folio sugerido
            Random rnd = new Random();

            int folioSugerido = rnd.Next(talonera.RangoInicio, talonera.RangoFin);

            ValeEntity valeValidacion = _context.Vale
                   .Where(v => v.Talonera.id == talonera.id && v.NumeroFolio == folioSugerido)
                   .FirstOrDefault();

            while (valeValidacion != null) 
            {
                folioSugerido = rnd.Next(talonera.RangoInicio, talonera.RangoFin);

                 valeValidacion = _context.Vale
                   .Where(v => v.Talonera.id == talonera.id && v.NumeroFolio == folioSugerido)
                   .FirstOrDefault();
            }

            ViewBag.FolioSugerido = folioSugerido;

            // termina generacion de numero de folio sugerido

            if (talonera.Empresa.NombreEmpresa != "Nombre Pendiente...")
                ViewBag.EmpresaDisplay = talonera.Empresa.NombreEmpresa;
            else
                ViewBag.EmpresaDisplay = talonera.Empresa.Email;

            return View();
        }

        // POST: Vales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Distribuidor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("id,Monto,ClienteId,EmpresaId,DistribuidorId,Fecha,FechaPrimerPago,status_vale,NumeroFolio,CantidadPagos")]
            ValeEntity valeEntity, int? idTalonera, int? idCliente)
        {
            DistribuidorEntity distribuidor = _context.Distribuidor
             .Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            TaloneraEntity talonera = _context.Talonera
                .Include(item => item.Empresa)
                .Where(t => t.id == idTalonera)
                .FirstOrDefault();

            ViewBag.Talonera = talonera;

            ClienteEntity cliente = _context.Cliente.Where(c => c.id == idCliente).FirstOrDefault();

            ViewBag.Cliente = cliente;

            if (ModelState.IsValid)
            {
                // evalua que el folio del vale ingresado este dentro del rango de la talonera a la que pertenece

                if (talonera == null ||
                    valeEntity.NumeroFolio < talonera.RangoInicio || valeEntity.NumeroFolio > talonera.RangoFin)
                    return RedirectToAction(nameof(ErrorVale));

                ValeEntity valeValidacion = _context.Vale
                    .Where(v => v.Talonera.id == talonera.id && v.NumeroFolio == valeEntity.NumeroFolio)
                    .FirstOrDefault();

                // evalua que el folio del vale ingresado no se repita dentro de la talonera a la que pertenece

                if (valeValidacion != null)
                    return RedirectToAction(nameof(ErrorVale));

                ValeEntity valeInsert = new ValeEntity
                {
                    Monto = valeEntity.Monto,
                    CantidadPagos = valeEntity.CantidadPagos,
                    //FechaPrimerPago = DateTime.Today,
                    FechaCreacion = DateTime.UtcNow,
                    status_vale = "Activo",
                    Talonera = talonera,
                    Distribuidor = distribuidor,
                    Empresa = talonera.Empresa,
                    Cliente = cliente,
                    NumeroFolio = valeEntity.NumeroFolio,
                    
                };

                _context.Vale.Add(valeInsert);
                await _context.SaveChangesAsync();

                float division = valeEntity.Monto / valeEntity.CantidadPagos;

                //GET: FECHA DE HOY
                DateTime FechaHoy = DateTime.Today;
                int Dia = FechaHoy.Day;
                int Mes = FechaHoy.Month;
                int Año = FechaHoy.Year;

                DateTime NuevaFecha = new DateTime(FechaHoy.Year, FechaHoy.Month, FechaHoy.Day);

                for (int i = 0; i < valeEntity.CantidadPagos; i++)
                {

                    //CONDICION PARA REDONDEAR LOS DIAS

                    if (FechaHoy.Day > 1 || FechaHoy.Day < 15)
                    { 
                        Debug.WriteLine("entre al primer if");
                        if (i == 0)
                        {
                            NuevaFecha = new DateTime(FechaHoy.Year, FechaHoy.Month, 15);
                        }
                        
                        FechaHoy = NuevaFecha;
                        Debug.WriteLine("Fecha quincena1" + NuevaFecha);
                       
                        _context.Pago.Add(new PagoEntity
                        {
                            Cantidad = division,
                            FechaLimite = NuevaFecha,
                            Vale = valeInsert

                        });

                        NuevaFecha = NuevaFecha.AddDays(15);

                    }
                    else if (FechaHoy.Day > 15)
                    {
                        Debug.WriteLine("entre al segundo if");
                        if (i == 0)
                        {
                            NuevaFecha = new DateTime(FechaHoy.Year, FechaHoy.Month, 1);
                            NuevaFecha.AddMonths(1);
                        }
                        
                        FechaHoy = NuevaFecha.ToLocalTime();
                        Console.WriteLine("Fecha quincena2" + FechaHoy);
                        FechaHoy = FechaHoy.AddDays(15);

                        _context.Pago.Add(new PagoEntity
                        {
                            Cantidad = division,
                            FechaLimite = FechaHoy,
                            Vale = valeInsert

                        });
                    }

                 
                }
                Debug.WriteLine("DIVISION");

                Debug.WriteLine(division);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(valeEntity);
        }

        // GET: Vales/Edit/5
        [Authorize(Roles = "Distribuidor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ValeEntity valeEntity = _context.Vale
                .Include(i => i.Cliente)
                .Where(v => v.id == id)
                .FirstOrDefault();

            ClienteEntity cliente = await _context.Cliente
                .Where(c => c.id == valeEntity.Cliente.id).FirstOrDefaultAsync();

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
        [Authorize(Roles = "Distribuidor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("id,Monto,DistribuidorId,EmpresaId,ClienteId,status_vale,Fecha,NumeroFolio")]
            ValeEntity valeEntity)
        {
            if (id != valeEntity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // evalua que el folio del vale ingresado este dentro del rango de la talonera a la que pertenece

                ValeEntity valeUntracked = _context.Vale.AsNoTracking()
                    .Include(i => i.Talonera)
                    .Where(v => v.id == valeEntity.id)
                    .FirstOrDefault();

                TaloneraEntity talonera = _context.Talonera
                    .Where(t => t.id == valeUntracked.Talonera.id)
                    .FirstOrDefault();

                if (talonera == null ||
                    valeEntity.NumeroFolio < talonera.RangoInicio || valeEntity.NumeroFolio > talonera.RangoFin)
                    return RedirectToAction(nameof(ErrorVale));

                // termina la evaluacion del rango de folio

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

        [Authorize(Roles = "Distribuidor")]
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

        [Authorize(Roles = "Distribuidor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id,
           [Bind("id,Monto,DistribuidorId,EmpresaId,ClienteId,status_vale,NumeroFolio,CantidadPagos,FechaPrimerPago,FechaCreacion")]
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
                    valeEntity.status_vale = "Inactivo";

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

        private bool PagoEntityExists(int id)
        {
            return _context.Pago.Any(e => e.id == id);
        }
    }
}
