using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class ClienteDistribuidor
    {
        public int ClienteId { get; set; }
        public ClienteEntity Cliente { get; set; }

        public int DistribuidorId { get; set; }
        public DistribuidorEntity Distribuidor { get; set; }
    }
}
