using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data
{
    public class DataContext: IdentityDbContext<UsuarioEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AdministradorEntity> Administrador { get; set; }
        public DbSet<EmpresaEntity> Empresa { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<ClienteEntity> Cliente { get; set; }

        public DbSet<DistribuidorEntity> Distribuidor { get; set; }
        public DbSet<ValeEntity> Vale { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
        }

        public DbSet<SAC_VALES.Web.Data.Entities.ValeEntity> ValeEntity { get; set; }

    }
}
