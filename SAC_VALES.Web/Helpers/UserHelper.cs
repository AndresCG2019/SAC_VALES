using Microsoft.AspNetCore.Identity;
using SAC_VALES.Common.Enums;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace SAC_VALES.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UsuarioEntity> _signInManager;

        public UserHelper(
           UserManager<UsuarioEntity> userManager,
           RoleManager<IdentityRole> roleManager,
           SignInManager<UsuarioEntity> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public async Task<IdentityResult> AddUserAsync(UsuarioEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<UsuarioEntity> AddUserAsync(AddUserViewModel model, string path)
        {
            UserType userType = new UserType();

            switch (model.UserTypeId)
            {
                case 1:
                    userType = UserType.Admin;
                    break;
                case 2:
                    userType = UserType.Empresa;
                    break;
                case 3:
                    userType = UserType.Distribuidor;
                    break;
                case 4:
                    userType = UserType.Cliente;
                    break;
            }

            UsuarioEntity userEntity = new UsuarioEntity
            {
                Email = model.Username,
                UserName = model.Username,
                // si se elige 1 es Admin, si se elige 2 es distribuidor
                UserType = userType
            };

            IdentityResult result = await _userManager.CreateAsync(userEntity, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            UsuarioEntity newUser = await GetUserByEmailAsync(model.Username);
            await AddUserToRoleAsync(newUser, userEntity.UserType.ToString());
            return newUser;
        }


        public async Task AddUserToRoleAsync(UsuarioEntity user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName); 
        }

        public async Task<IdentityResult> ChangePasswordAsync(UsuarioEntity user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }

        }

        public async Task<UsuarioEntity> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);

        }

        public async Task<bool> IsUserInRoleAsync(UsuarioEntity user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Contraseña,
                model.Recuerdame,
                false);

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(UsuarioEntity user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
