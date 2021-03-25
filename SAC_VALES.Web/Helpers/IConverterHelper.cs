using System.Collections.Generic;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Helpers
{
    public interface IConverterHelper
    {
        UserResponse ToDistUserResponse(UsuarioEntity user, DistribuidorEntity dist);
        UserResponse ToClieUserResponse(UsuarioEntity user, ClienteEntity clie);
        List <ValeResponse> ToValesResponse(List<ValeEntity> vales);
        DistResponse ToDistResponse(DistribuidorEntity dist);
        ClieResponse ToClieResponse(ClienteEntity cliente);
        EmpresaResponse ToEmpResponse(EmpresaEntity empresa);
        TaloneraResponse ToTaloneraResponse(TaloneraEntity talonera);
    }
}
