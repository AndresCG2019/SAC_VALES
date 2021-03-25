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

     

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
                //Enlista los vales activos
                List<ValeEntity> valesActivos = await _context.Vale
                   .Where(v => v.Pagado == true)
                   .ToListAsync();

                //Enlista los vales Falsos
                List<ValeEntity> valesFalsos = await _context.Vale
                  .Where(v => v.Pagado == false)
                  .ToListAsync();

                int iActivos = 0; // Iterador de vales Pagados
                int iFalsos = 0; // Iterador de vales No Pagados

                for (int i = 0; i < valesActivos.Count; i++)
                {
                    iActivos++; // Acumula los vales que son pagados

                }

                for (int i = 0; i < valesFalsos.Count; i++)
                {
                    iFalsos++; // Acumula los vales que son no pagados

                }

                ViewBag.valesActivos = iActivos;
                ViewBag.valesFalsos = iFalsos;
                ViewBag.texto1 = "Dashboard del distribuidor";
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
