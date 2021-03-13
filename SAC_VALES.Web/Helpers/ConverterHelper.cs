using System.Collections.Generic;
using System.Linq;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public UserResponse ToUserResponse(UsuarioEntity user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Email = user.Email,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType
            };
        }
    }
}
