using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class PagoEntity
    {
        public int id { get; set; }

        [Display(Name = "Cantidad a pagar")]
        public float Cantidad { get; set; }
       
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha limite")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime FechaLimite { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de pago")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime FechaLimiteLocal => FechaLimite.ToLocalTime();

        public bool Pagado { get; set; }

        public int Valeid { get; set; }

        public ValeEntity Vale { get; set; }
    }
}
