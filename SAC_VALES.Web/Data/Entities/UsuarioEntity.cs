using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SAC_VALES.Common.Enums;

namespace SAC_VALES.Web.Data.Entities
{
    public class UsuarioEntity : IdentityUser
    {
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellidos { get; set; }

        [Display(Name = "Direccion")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Direccion { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellidos}";

    }
}
