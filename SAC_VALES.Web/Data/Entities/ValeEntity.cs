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
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DistribuidorEntity Distribuidor { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public EmpresaEntity Empresa { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ClienteEntity Cliente { get; set; }

    }
}
