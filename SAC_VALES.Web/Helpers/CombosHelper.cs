using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public IEnumerable<SelectListItem> GetComboRolesAdmin()
        {
            // seguir el orden de valor de cada rol, no reestablecerlo en cada metodo
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "[Select a role...]" },
                new SelectListItem { Value = "1", Text = "Admin" },
                new SelectListItem { Value = "2", Text = "Empresa" }
            };
            return list;
        }

        public IEnumerable<SelectListItem> GetComboRolesEmpresa()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "3", Text = "Distribuidor" }
            };
            return list;
        }

        public IEnumerable<SelectListItem> GetComboRolesDistribuidor()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "4", Text = "Cliente" }
            };
            return list;
        }
    }
}
