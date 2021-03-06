using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
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

        public async Task<IActionResult> Index()
        {
            DateTime hoyDate = DateTime.UtcNow.ToLocalTime();
            string hoyString = DateTime.UtcNow.ToLocalTime().ToShortDateString();

            var pagos = await _context.Pago
                .Where(p => p.FechaLimiteLocal.Date == hoyDate.Date && p.Vale.Distribuidor.Email == User.Identity.Name)
                .Include(p => p.Vale.Cliente)
                .Include(p => p.Vale.Talonera.Empresa)
                .ToListAsync();

            ViewBag.FechaDisplay = hoyString;

            return View(pagos);
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
    }
}
