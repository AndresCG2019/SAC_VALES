using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class PagoResponse
    {
        public int id { get; set; }

        public float Cantidad { get; set; }

        public DateTime FechaLimite { get; set; }

        public DateTime FechaLimiteLocal => FechaLimite.ToLocalTime();

        public string FechalString => FechaLimiteLocal.ToShortDateString();

        public bool Pagado { get; set; }
    }
}
