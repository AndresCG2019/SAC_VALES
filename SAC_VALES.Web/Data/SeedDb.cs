using SAC_VALES.Common.Enums;
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

            await CheckUserAsync("1010", "Juan", "Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Calle Luna Calle Pereyra", UserType.Admin);

        }

        private async Task<UsuarioEntity> CheckUserAsync(
           string document,
           string firstName,
           string lastName,
           string email,
           string phone,
           string address,
           UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UsuarioEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
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
        }

        private async Task CheckAdminsAsync()
        {
            if (_dataContext.Administrador.Any()) return;
            _dataContext.Administrador.Add(new AdministradorEntity 
            { 
                Nombre = "Juan",
                ApellidoP = "Perez",
                ApellidoM = "Sanchez",
                Telefono = "6181658123",
                status = true

            });

            _dataContext.Administrador.Add(new AdministradorEntity
            {
                Nombre = "Pedro",
                ApellidoP = "Casas",
                ApellidoM = "Rosas",
                Telefono = "6181928743",
                status = true

            });

            await _dataContext.SaveChangesAsync();

        }
    }
}
