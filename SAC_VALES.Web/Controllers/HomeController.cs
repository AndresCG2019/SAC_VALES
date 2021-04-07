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
                .Where(t => t.Distribuidor.Email == User.Identity.Name)
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
                data.EmailEmpresa = taloneras[i].Empresa.Email + ": " +
                    taloneras[i].RangoInicio + " - " + taloneras[i].RangoFin;

                talonerasChartData.Add(data);

            }

            return Json(talonerasChartData);
        }

        public async Task <IActionResult> getAdeudos()
        {
            //OBTENER USUARIO LOGUEADO
            DistribuidorEntity distribuidor = _context.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();
            //OBTENER CORREOS DE LOS CLIENTES ASOCIADOS AL DISTRIBUIDOR LOGUEADO
                List<ClienteDistribuidor> clienteDistribuidor = await _context.ClienteDistribuidor
                    .Include(item => item.Cliente)
                    .Where(cd => cd.DistribuidorId == distribuidor.id)
                    .ToListAsync();

                var correos = clienteDistribuidor[0].Cliente.Email;

            List<AdeudosClientesChartData> AdeudosChartData = new List<AdeudosClientesChartData>();

            //adeudos
            /*List<PagoEntity> adeudosClienteDistribuidor = await _context.Pago
                   .Where(p => p.Pagado == false)
                   .ToListAsync();
            var adeudos = adeudosClienteDistribuidor[0].Cantidad;

            AdeudosClientesChartData data1 = new AdeudosClientesChartData();
            for (int i = 0; i < adeudosClienteDistribuidor.Count; i++)
            {
                //OBTENER LOS SALDOS ADEUDADOS
                
                adeudos = adeudosClienteDistribuidor[i].Cantidad;
                
            }

            double acumAdeudos = 0.0;
            acumAdeudos = acumAdeudos + adeudos;
            data1.AdeudoCliente = acumAdeudos;
            Debug.WriteLine("adeudos: " + acumAdeudos);
            AdeudosChartData.Add(data1);*/
            //end adeudos

            for (int i = 0; i < clienteDistribuidor.Count; i++)
            {
                AdeudosClientesChartData data = new AdeudosClientesChartData();
                correos = clienteDistribuidor[i].Cliente.Email;
                data.EmailCliente = correos;
                //AdeudosChartData.Add(data);
                Debug.WriteLine("correos: " + correos);
                AdeudosChartData.Add(data);
            }


            return Json(AdeudosChartData);

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

                List<PagoEntity> pagosTrue = await _context.Pago
                   .Where(p => p.Pagado == true)
                   .ToListAsync();
                List<PagoEntity> pagosFalse = await _context.Pago
                  .Where(p => p.Pagado == false)
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


                return View();
            }
            else if (User.IsInRole("Cliente"))
            {
                ViewBag.texto2 = "Dashboard del cliente";
                return View();
            }

            return View();
        }

    
    } 
}
