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

        [StringLength(90, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Apellidos { get; set; }

        [StringLength(17, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Telefono { get; set; }

        [Display(Name = "Email")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Email { get; set; }

        public bool status { get; set; }

        public UsuarioEntity AdminAuth { get; set; }

    }
}
