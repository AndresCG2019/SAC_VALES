﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class TaloneraResponse
    {
        public int id { get; set; }

        public int RangoInicio { get; set; }

        public int RangoFin { get; set; }

        public string Display { get; set; }

        public EmpresaResponse Empresa { get; set; }
    }
}
