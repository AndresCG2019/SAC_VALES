﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class EmpresaEntity
    {
        public int id { get; set; }
        public string NombreEmpresa { get; set; }
        public UsuarioEntity representante { get; set; }
    }
}
