using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class CreateValeRequest
    {
        public int NumeroFolio { get; set; }
        public float Monto { get; set; }
        public DateTime FechaPrimerPago { get; set; }
        public DateTime FechaPrimerPagoLocal => FechaPrimerPago.ToLocalTime();
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCreacionlocal => FechaCreacion.ToLocalTime();
        public int CantidadPagos { get; set; }
        public string StatusVale { get; set; }
        public int DistId { get; set; }
        public int EmpresaId { get; set; }
        public int ClienteId { get; set; }
        public int TaloneraId { get; set; }
    }
}
