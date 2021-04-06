using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public UserResponse ToDistUserResponse(UsuarioEntity user, DistribuidorEntity dist)
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
                UserType = user.UserType,
                Dist = ToDistResponse(dist)
            };
        }

        public UserResponse ToClieUserResponse(UsuarioEntity user, ClienteEntity clie)
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
                UserType = user.UserType,
                Clie = ToClieResponse(clie)
            };
        }

        public List <ValeResponse> ToValesResponse(List<ValeEntity> vales, List<PagoEntity> pagos)
        {
            return vales.Select(v => new ValeResponse
            {
                id = v.id,
                NumeroFolio = v.NumeroFolio,
                Monto = v.Monto,
                FechaCreacion = v.FechaCreacion,
                FechaPrimerPago = v.FechaPrimerPago,
                CantidadPagos = v.CantidadPagos,
                Pagado = v.Pagado,
                status_vale = v.status_vale,
                Dist = ToDistResponse(v.Distribuidor),
                Cliente = ToClieResponse(v.Cliente),
                Empresa = ToEmpResponse(v.Empresa),
                Talonera = ToTaloneraResponse(v.Talonera),
                Pagos = ToPagosResponse(pagos, v.id)


            }).ToList();
        }

        public List<PagoResponse> ToPagosResponse(List<PagoEntity> pagos, int ValeId)
        {
            if (pagos == null)
            {
                return null;
            }

            List<PagoResponse> pagosResponse = new List<PagoResponse>();
            PagoResponse pago = new PagoResponse();

            for (int i = 0; i < pagos.Count; i++)
            {
                if (pagos[i].Vale.id == ValeId)
                {
                    pago.id = pagos[i].id;
                    pago.Cantidad = pagos[i].Cantidad;
                    pago.FechaLimite = pagos[i].FechaLimite;
                    pago.Pagado = pagos[i].Pagado;

                    pagosResponse.Add(pago);

                    pago = new PagoResponse();

                    Debug.WriteLine("LLEGUE");
                }
            }

            return pagosResponse;
        }

        public DistResponse ToDistResponse(DistribuidorEntity dist)
        {
            if (dist == null)
            {
                return null;
            }

            return new DistResponse
            {
                id = dist.id,
                Nombre = dist.Nombre,
                Apellidos = dist.Apellidos,
                Direccion = dist.Direccion,
                Telefono = dist.Telefono,
                Email = dist.Email,
                Status = dist.StatusDistribuidor
            };
        }

        public ClieResponse ToClieResponse(ClienteEntity cliente)
        {
            if (cliente == null)
            {
                return null;
            }

            return new ClieResponse
            {
                id = cliente.id,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Status = cliente.status_cliente
            };
        }

        public EmpresaResponse ToEmpResponse(EmpresaEntity empresa)
        {
            if (empresa == null)
            {
                return null;
            }

            return new EmpresaResponse
            {
                id = empresa.id,
                NombreEmpresa = empresa.NombreEmpresa,
                NombreRepresentante = empresa.NombreRepresentante,
                ApellidosRepresentante = empresa.ApellidosRepresentante,
                Direccion = empresa.Direccion,
                TelefonoRepresentante = empresa.TelefonoRepresentante,
                Email = empresa.Email,
            };
        }

        public TaloneraResponse ToTaloneraResponse(TaloneraEntity talonera)
        {
            if (talonera == null)
            {
                return null;
            }

            return new TaloneraResponse
            {
                id = talonera.id,
                RangoInicio = talonera.RangoInicio,
                RangoFin = talonera.RangoFin,
                Empresa = ToEmpResponse(talonera.Empresa)

            };
        }

    }
}
