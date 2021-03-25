using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class EmpresaResponse
    {
        public int id { get; set; }
        public string NombreEmpresa { get; set; }

        public string NombreRepresentante { get; set; }

        public string ApellidosRepresentante { get; set; }

        public string TelefonoRepresentante { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }
    }
}
