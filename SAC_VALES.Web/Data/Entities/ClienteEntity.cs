using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class ClienteEntity
    {
        public int id { get; set; }
        public bool status_cliente { get; set; }
        public UsuarioEntity Distribuidor { get; set; }
        public UsuarioEntity Cliente { get; set; }
    }
}
