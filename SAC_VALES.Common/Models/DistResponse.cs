﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAC_VALES.Common.Models
{
   public class DistResponse
    {
        public int id { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public bool Status { get; set; }

        public string DistDisplay => Nombre + " " + Apellidos;
    }
}
