using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SAC_VALES.Common.Enums;

namespace SAC_VALES.Web.Data.Entities
{
    public class AdministradorEntity
    {
        public int id { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string ApellidoP { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string ApellidoM { get; set; }

        [StringLength(17, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Telefono { get; set; }

        public UserType userType { get; set; } // SEGUIR IMPLEMENTANDO AUTH PROBAR ROLE Y DESPUES REGISTER USER

        public bool status { get; set; }


    }
}
