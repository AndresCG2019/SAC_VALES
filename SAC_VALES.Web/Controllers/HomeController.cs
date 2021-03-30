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
                data.EmailEmpresa = taloneras[i].Empresa.Email + ": " + 
                    taloneras[i].RangoInicio + " - " + taloneras[i].RangoFin;

                talonerasChartData.Add(data);

            }

            return Json(talonerasChartData);
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
                   .Where(v => v.Pagado == true)
                   .ToListAsync();

                
                List<ValeEntity> valesFalsos = await _context.Vale
                  .Where(v => v.Pagado == false)
                  .ToListAsync();

                ViewBag.valesActivos = valesActivos.Count;
                ViewBag.valesFalsos = valesFalsos.Count;
                ViewBag.texto1 = "Dashboard del distribuidor";

                // DATOS PARA SIGUIENTE GRAFICA ...

                return View();
            }

            return View("TestView");
        }

    
    }
}
