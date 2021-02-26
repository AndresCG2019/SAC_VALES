﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data.Entities
{
    public class ValeEntity
    {
        public int id { get; set; }

        [Range(1, 2000, ErrorMessage = "Por favor, ingrese un número de folio válido")]
        [Display(Name = "Número de folio")]
        [Required(ErrorMessage = "Es necesario ingresar un número de folio")]
        public int NumeroFolio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Monto { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación de talonera")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime FechaLocal => Fecha.ToLocalTime();

        [Range(1, 48, ErrorMessage = "Por favor, ingrese un número de pagos válido")]
        [Display(Name = "Cantidad de pagos quincenales")]
        [Required(ErrorMessage = "Es necesario ingresar una cantidad de pagos")]
        public int CantidadPagos { get; set; }

        public string status_vale { get; set; }
        public int dia { get; set; }
        public string Mes { get; set; }
        public int Año { get; set; }

        public DistribuidorEntity Distribuidor { get; set; }

        public EmpresaEntity Empresa { get; set; }

        public ClienteEntity Cliente { get; set; }

        public TaloneraEntity Talonera { get; set; }

    }
}
