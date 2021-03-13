using System.Collections.Generic;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Helpers
{
    public interface IConverterHelper
    {
        UserResponse ToUserResponse(UsuarioEntity user);
    }
}
