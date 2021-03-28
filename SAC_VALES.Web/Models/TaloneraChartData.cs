using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Models
{
    public class TaloneraChartData
    {
        public int NumFolios { get; set; }

        public int FoliosDisponible { get; set; }

        public int FoliosOcupados { get; set; }

        public string EmailEmpresa { get; set; }
    }
}
