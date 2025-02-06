﻿// <auto-generated />
using System;
using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CajaAhorrosBackend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250103163406_AddClientAccountRelation")]
    partial class AddClientAccountRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CajaAhorrosBackend.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdCliente"));

                    b.Property<string>("CorreoElectronico")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Rol")
                        .HasColumnType("text");

                    b.HasKey("IdCliente");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("CajaAhorrosBackend.Models.CuentaAhorro", b =>
                {
                    b.Property<int>("IdCuenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdCuenta"));

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer");

                    b.Property<float>("Saldo")
                        .HasColumnType("real");

                    b.HasKey("IdCuenta");

                    b.HasIndex("IdUsuario");

                    b.ToTable("CuentasAhorro");
                });

            modelBuilder.Entity("CajaAhorrosBackend.Models.Transaccion", b =>
                {
                    b.Property<int>("IdTransaccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTransaccion"));

                    b.Property<DateTime>("FechaTransaccion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("IdCuenta")
                        .HasColumnType("integer");

                    b.Property<float>("Monto")
                        .HasColumnType("real");

                    b.Property<int>("TipoTransaccion")
                        .HasColumnType("integer");

                    b.HasKey("IdTransaccion");

                    b.ToTable("Transacciones");
                });

            modelBuilder.Entity("CajaAhorrosBackend.Models.CuentaAhorro", b =>
                {
                    b.HasOne("CajaAhorrosBackend.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });
#pragma warning restore 612, 618
        }
    }
}
