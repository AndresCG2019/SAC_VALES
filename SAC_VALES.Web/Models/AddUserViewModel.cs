using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Models
{
    public class AddUserViewModel : EditUserViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [EmailAddress]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmación de Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Registrar como")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un rol.")]
        public int UserTypeId { get; set; }

        public int navId { get; set; }

        public IEnumerable<SelectListItem> UserTypes { get; set; }

    }
}
