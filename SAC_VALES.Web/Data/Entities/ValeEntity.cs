using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class ValeEntity
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Monto { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha Formato Local")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime FechaLocal => Fecha.ToLocalTime();

        public string status_vale { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DistribuidorId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ClienteId { get; set; }

    }
}
