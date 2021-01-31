using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class DistribuidorEntity 
    {
        public int id { get; set; }
        public string EmpresaVinculada { get; set; }
        public UsuarioEntity Usuario { get; set; }
    }
}
