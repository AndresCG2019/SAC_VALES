using SAC_VALES.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Prism.Helpers
{
    public static class CombosHelper
    {
        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Distribuidor" },
                new Role { Id = 2, Name = "Cliente" }
            };
        }

    }
}
