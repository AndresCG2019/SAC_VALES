using Microsoft.AspNetCore.Identity;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SAC_VALES.Web.Helpers
{
    public interface IUserHelper
    {
        Task<UsuarioEntity> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(UsuarioEntity user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(UsuarioEntity user, string roleName);

        Task<bool> IsUserInRoleAsync(UsuarioEntity user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();


    }
}
