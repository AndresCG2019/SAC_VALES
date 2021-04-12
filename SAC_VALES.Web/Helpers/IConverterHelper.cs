using System.Collections.Generic;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Helpers
{
    public interface IConverterHelper
    {
        UserResponse ToDistUserResponse(UsuarioEntity user, DistribuidorEntity dist);
        UserResponse ToClieUserResponse(UsuarioEntity user, ClienteEntity clie);
        List <ValeResponse> ToValesResponse(List<ValeEntity> vales, List<PagoEntity> pagos);
        DistResponse ToDistResponse(DistribuidorEntity dist);
        ClieResponse ToClieResponse(ClienteEntity cliente);
        EmpresaResponse ToEmpResponse(EmpresaEntity empresa);
        TaloneraResponse ToTaloneraResponse(TaloneraEntity talonera);
        List<TaloneraResponse> ToTalonerasResponse(List<TaloneraEntity> taloneras);
        ClieResponse ToClientResponse(ClienteDistribuidor cliente);
        List<ClieResponse> ToClientsResponse(List<ClienteDistribuidor> clientes);
        List<PagoResponse> ToPagosResponse(List<PagoEntity> pagos, int ValeId);
    }
}
