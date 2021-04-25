using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using SAC_VALES.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public HomeController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }


        public ActionResult getTaloneras() 
        {
            List<TaloneraEntity> taloneras = _context.Talonera
                .Where(t => t.Distribuidor.Email == User.Identity.Name && t.StatusTalonera == "Activo")
                .Include(t => t.Empresa)
                .ToList();

            List<TaloneraChartData> talonerasChartData = new List<TaloneraChartData>();

            for (int i = 0; i < taloneras.Count; i++)
            {

                List<ValeEntity> vales = _context.Vale
                    .Where(v => v.Talonera.id == taloneras[i].id)
                    .ToList();

                TaloneraChartData data = new TaloneraChartData();

                data.NumFolios = taloneras[i].RangoFin - taloneras[i].RangoInicio;
                data.FoliosOcupados = vales.Count;
                data.FoliosDisponible = data.NumFolios - data.FoliosOcupados;
                data.EmailEmpresa = taloneras[i].Empresa.NombreEmpresa + ": " + 
                    taloneras[i].RangoInicio + " - " + taloneras[i].RangoFin;

                talonerasChartData.Add(data);

            }

            if (taloneras.Count == 0)
            {
                return View("TestView");
            }
            else
            {
                return Json(talonerasChartData);
            }
        }

        public async Task<IActionResult> getAdeudos()
        {
            //OBTENER USUARIO LOGUEADO
            DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

            List<AdeudosClientesChartData> AdeudosChartData = new List<AdeudosClientesChartData>();


            //Obtener adeudos
            List<PagoEntity> adeudos = _context.Pago
                .Include(p => p.Vale.Cliente)
                .Include(p => p.Vale.Talonera)
                .Include(p => p.Vale.Talonera.Empresa)
                .Where(p => p.Distribuidor.id == distribuidor.id && p.Pagado == false)
                .ToList();


            for (int i = 0; i < adeudos.Count; i++)
            {

                AdeudosClientesChartData data = new AdeudosClientesChartData();
                data.AdeudoCliente = adeudos[i].Cantidad;
                data.EmailCliente = adeudos[i].Vale.Cliente.Nombre + " " + adeudos[i].Vale.Cliente.Apellidos + ": " + adeudos[i].Vale.Talonera.Empresa.NombreEmpresa +
                   " " + adeudos[i].Vale.Talonera.RangoInicio.ToString() +
                  "-" + adeudos[i].Vale.Talonera.RangoFin.ToString();




                AdeudosChartData.Add(data);
            }


            //obtener correos
            List<ClienteDistribuidor> clienteDistribuidor = await _context.ClienteDistribuidor
                    .Include(item => item.Cliente)
                    .Where(cd => cd.DistribuidorId == distribuidor.id)
                    .ToListAsync();



            Debug.WriteLine("apara aqui");


            if (adeudos.Count == 0)
            {
                return View("TestView");
            }
            else
            {
                return Json(AdeudosChartData);
            }

        }
        public IActionResult About()
        {
            ViewData["Title"] = "EN CONSTRUCCION.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });        
                
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (User.IsInRole("Distribuidor"))
            {

                // DATOS PARA LA GRAFICA DE VALES ACTIVOS E INACTIVOS

                List<ValeEntity> valesActivos = await _context.Vale
                   .Where(v => v.Pagado == true && v.Distribuidor.Email == User.Identity.Name)
                   .ToListAsync();


                List<ValeEntity> valesFalsos = await _context.Vale
                  .Where(v => v.Pagado == false && v.Distribuidor.Email == User.Identity.Name)
                  .ToListAsync();

                ViewBag.valesActivos = valesActivos.Count;
                ViewBag.valesFalsos = valesFalsos.Count;


                // DATOS PARA PAGOS GRAFICA...

                //DistribuidorEntity distribuidor1 = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();

                List<PagoEntity> pagosTrue = await _context.Pago
                    .Include(p => p.Distribuidor)
                   .Where(p => p.Pagado == true && p.Distribuidor.Email == User.Identity.Name)
                   .ToListAsync();
                List<PagoEntity> pagosFalse = await _context.Pago
                  .Where(p => p.Pagado == false && p.Distribuidor.Email == User.Identity.Name)
                  .ToListAsync();


                ViewBag.pagostrue = pagosTrue.Count;
                ViewBag.pagosfalse = pagosFalse.Count;



                //Gráfica de adeudos

                /*DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();
                List<PagoEntity> adeudosClienteDistribuidor = await _context.Pago
                    .Where(p => p.Pagado == false)
                    .ToListAsync();
                var adeudos = adeudosClienteDistribuidor[0].Cantidad;
                
                for (int i = 0; i < adeudosClienteDistribuidor.Count; i++)
                {
                    adeudos = adeudosClienteDistribuidor[i].Cantidad;
                    Debug.WriteLine("adeudos: " + adeudos);
                }*/

                if (valesActivos.Count == 0 && valesFalsos.Count == 0 && pagosFalse.Count == 0 && pagosTrue.Count == 0)
                {
                    return View("TestView");
                }
                else
                {
                    return View();
                }

            }
            else if (User.IsInRole("Cliente"))
            {
                ViewBag.texto2 = "Dashboard del cliente";
                return View("TestView");
            }

            return View("TestView");
        }

    
    }
}
