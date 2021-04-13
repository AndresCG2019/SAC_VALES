using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class CreateTaloneraRequest
    {
        public int EmpresaId { get; set; }
        public int RangoInicio { get; set; }
        public int RangoFin { get; set; }
        public int DistId { get; set; }
    }
}
