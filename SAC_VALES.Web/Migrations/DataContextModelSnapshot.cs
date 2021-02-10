﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SAC_VALES.Web.Data;

namespace SAC_VALES.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.AdministradorEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminAuthId");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(90);

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(17);

                    b.Property<bool>("status");

                    b.HasKey("id");

                    b.HasIndex("AdminAuthId");

                    b.ToTable("Administrador");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.ClienteEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ClienteAuthId");

                    b.Property<string>("Direccion")
                        .HasMaxLength(100);

                    b.Property<int?>("Distribuidorid");

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("status_cliente");

                    b.HasKey("id");

                    b.HasIndex("ClienteAuthId");

                    b.HasIndex("Distribuidorid");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.DistribuidorEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Direccion")
                        .HasMaxLength(100);

                    b.Property<string>("DistribuidorAuthId");

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<int?>("EmpresaVinculadaid");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("StatusDistribuidor");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("id");

                    b.HasIndex("DistribuidorAuthId");

                    b.HasIndex("EmpresaVinculadaid");

                    b.ToTable("Distribuidor");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.EmpresaEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApellidosRepresentante")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Direccion")
                        .HasMaxLength(100);

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("EmpresaAuthId");

                    b.Property<string>("NombreEmpresa")
                        .HasMaxLength(50);

                    b.Property<string>("NombreRepresentante")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TelefonoRepresentante")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("id");

                    b.HasIndex("EmpresaAuthId");

                    b.ToTable("Empresa");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.UsuarioEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("UserType");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.ValeEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClienteId");

                    b.Property<int>("DistribuidorId");

                    b.Property<int>("EmpresaId");

                    b.Property<DateTime>("Fecha");

                    b.Property<float>("Monto");

                    b.HasKey("id");

                    b.ToTable("ValeEntity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.AdministradorEntity", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity", "AdminAuth")
                        .WithMany()
                        .HasForeignKey("AdminAuthId");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.ClienteEntity", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity", "ClienteAuth")
                        .WithMany()
                        .HasForeignKey("ClienteAuthId");

                    b.HasOne("SAC_VALES.Web.Data.Entities.DistribuidorEntity", "Distribuidor")
                        .WithMany()
                        .HasForeignKey("Distribuidorid");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.DistribuidorEntity", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity", "DistribuidorAuth")
                        .WithMany()
                        .HasForeignKey("DistribuidorAuthId");

                    b.HasOne("SAC_VALES.Web.Data.Entities.EmpresaEntity", "EmpresaVinculada")
                        .WithMany()
                        .HasForeignKey("EmpresaVinculadaid");
                });

            modelBuilder.Entity("SAC_VALES.Web.Data.Entities.EmpresaEntity", b =>
                {
                    b.HasOne("SAC_VALES.Web.Data.Entities.UsuarioEntity", "EmpresaAuth")
                        .WithMany()
                        .HasForeignKey("EmpresaAuthId");
                });
#pragma warning restore 612, 618
        }
    }
}
