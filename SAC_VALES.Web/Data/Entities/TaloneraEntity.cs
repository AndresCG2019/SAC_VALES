using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class TaloneraEntity
    {
        public int id { get; set; }
        [Range(1, 1000, ErrorMessage = "Por favor, ingrese un número válido")]
        [Display(Name = "Primer número de folio")]
        [Required(ErrorMessage = "Es necesario ingresar un inicio del rango")]
        public int RangoInicio { get; set; }
        [Range(1, 2000, ErrorMessage = "Por favor, ingrese un número válido")]
        [Display(Name = "Ultimo número de folio")]
        [Required(ErrorMessage = "Es necesario ingresar un final del rango")]
        public int RangoFin { get; set; }
        public EmpresaEntity Empresa { get; set; }

        public DistribuidorEntity Distribuidor { get; set; }
    }
}
