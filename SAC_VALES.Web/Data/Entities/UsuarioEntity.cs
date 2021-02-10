using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SAC_VALES.Common.Enums;

namespace SAC_VALES.Web.Data.Entities
{
    public class UsuarioEntity : IdentityUser
    {
        [Display(Name = "User Type")]
        public UserType UserType { get; set; }
    }
}
