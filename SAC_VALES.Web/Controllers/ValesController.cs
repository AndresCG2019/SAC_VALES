using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(i => i.Empresa)
                .Include(i => i.Cliente)
                .Include(i => i.Talonera)
                .Where(v => v.Distribuidor.id == distribuidor.id && v.status_vale == "Activo")
                .ToListAsync());
        }

        public async Task<IActionResult> SelectTalonera()
        {
            return View(await _context.Talonera
                .Where(t => t.Distribuidor.Email == User.Identity.Name)
                .Include(i => i.Empresa)
                .ToListAsync());
        }

        public async Task<IActionResult> SelectCliente(int? id)
        {
            DistribuidorEntity distribuidor = _context.Distribuidor
                .Where(d => d.Email == User.Identity.Name)
                .FirstOrDefault();

            ViewBag.idTalonera = id;

            Debug.WriteLine("id en select cliente");
            Debug.WriteLine(id);

            return View(await _context.ClienteDistribuidor
                .Include(item => item.Cliente)
                .Where(cd => cd.DistribuidorId == distribuidor.id)
                .ToListAsync());
        }

        public IActionResult ErrorVale()
        {
            Debug.WriteLine("ENTRE A ERROR VALE");
            return View();
        }

        // GET: Vales/Create
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

            if (talonera.Empresa.NombreEmpresa != "Nombre Pendiente...")
                ViewBag.EmpresaDisplay = talonera.Empresa.NombreEmpresa;
            else
                ViewBag.EmpresaDisplay = talonera.Empresa.Email;

            return View();
        }

        // POST: Vales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Monto,ClienteId,EmpresaId,DistribuidorId,Fecha,status_vale,NumeroFolio")]
        ValeEntity valeEntity, int? idTalonera, int? idCliente)
        {

            Debug.WriteLine("ID DE LA TALONERA EN TASK");
            Debug.WriteLine(idTalonera);

            Debug.WriteLine("ID DEL CLIENTE EN TASK");
            Debug.WriteLine(idCliente);


            DistribuidorEntity distribuidor = _context.Distribuidor
             .Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            TaloneraEntity talonera = _context.Talonera
                .Include(item => item.Empresa)
                .Where(t => t.id == idTalonera)
                .FirstOrDefault();

            ClienteEntity cliente = _context.Cliente.Where(c => c.id == idCliente).FirstOrDefault();

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

                _context.Vale.Add(new ValeEntity
                {
                    Monto = valeEntity.Monto,
                    Fecha = DateTime.UtcNow,
                    status_vale = "Activo",
                    Talonera = talonera,
                    Distribuidor = distribuidor,
                    Empresa = talonera.Empresa,
                    Cliente = cliente,  
                    NumeroFolio = valeEntity.NumeroFolio
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(valeEntity);
        }

        // GET: Vales/Edit/5
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

                Debug.WriteLine("VALE UNTRACKED ID EN EDIT SUBMIT");
                Debug.WriteLine(valeUntracked.id);

                TaloneraEntity talonera = _context.Talonera
                    .Where(t => t.id == valeUntracked.Talonera.id)
                    .FirstOrDefault();

                Debug.WriteLine("TALONERA ID EN EDIT SUBMIT");
                Debug.WriteLine(talonera.id);

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
        public async Task<IActionResult> Eliminar(int id,
           [Bind("id,Monto,DistribuidorId,EmpresaId,ClienteId,status_vale,NumeroFolio")]
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
    }
}
