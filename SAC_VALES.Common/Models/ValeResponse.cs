using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class ValeResponse
    {
        public int id { get; set; }

        public int NumeroFolio { get; set; }

        public float Monto { get; set; }

        public DateTime FechaPrimerPago { get; set; }

        public DateTime FechaPagoLocal => FechaPrimerPago.ToLocalTime();

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaCreacionLocal => FechaCreacion.ToLocalTime();

        public int CantidadPagos { get; set; }

        public bool Pagado { get; set; }

        public string status_vale { get; set; }
    }
}
