using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AdministradorEntity> Administrador { get; set; }
        public DbSet<EmpresaEntity> Empresa { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AdministradorEntity>()
                .HasIndex(a => a.id)
                .IsUnique();
        }

    }
}
