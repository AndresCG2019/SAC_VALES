﻿using SAC_VALES.Common.Enums;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckAdminsAsync();
            await CheckRolesAsync();

            await CheckUserAsync("jzuluaga55@gmail.com", "350 634 2747", UserType.Admin);
        }

        private async Task<UsuarioEntity> CheckUserAsync(
           string email,
           string phone,
           UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UsuarioEntity
                {   
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Distribuidor.ToString());
            await _userHelper.CheckRoleAsync(UserType.Empresa.ToString());
            await _userHelper.CheckRoleAsync(UserType.Cliente.ToString());
        }

        private async Task CheckAdminsAsync()
        {
            if (_dataContext.Administrador.Any()) return;
            _dataContext.Administrador.Add(new AdministradorEntity 
            { 
                Nombre = "Juan",
                Apellidos = "Suarez",
                Telefono = "6181658123",
                Email = "juan@yopmail.com",
                status = true

            });
            await _dataContext.SaveChangesAsync();

        }
    }
}
