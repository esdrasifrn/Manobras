// <auto-generated />
using System;
using Manobra.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Manobra.Infra.Migrations
{
    [DbContext(typeof(ManobraContext))]
    [Migration("20211214034353_inicial")]
    partial class inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Manobra.Domain.Entities.Carro", b =>
                {
                    b.Property<int>("CarroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Marca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modelo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Placa")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CarroId");

                    b.ToTable("Carro");
                });

            modelBuilder.Entity("Manobra.Domain.Entities.CarroManobrista", b =>
                {
                    b.Property<int>("CarroId")
                        .HasColumnType("int");

                    b.Property<int>("ManobristaId")
                        .HasColumnType("int");

                    b.HasKey("CarroId", "ManobristaId");

                    b.HasIndex("ManobristaId");

                    b.ToTable("CarroManobrista");
                });

            modelBuilder.Entity("Manobra.Domain.Entities.Manobrista", b =>
                {
                    b.Property<int>("ManobristaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CPF")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ManobristaId");

                    b.ToTable("Monobrista");
                });

            modelBuilder.Entity("Manobra.Domain.Entities.CarroManobrista", b =>
                {
                    b.HasOne("Manobra.Domain.Entities.Carro", "CarroManobrado")
                        .WithMany("CarroManobristas")
                        .HasForeignKey("CarroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Manobra.Domain.Entities.Manobrista", "ResponsavelPelaManobra")
                        .WithMany("CarroManobristas")
                        .HasForeignKey("ManobristaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarroManobrado");

                    b.Navigation("ResponsavelPelaManobra");
                });

            modelBuilder.Entity("Manobra.Domain.Entities.Carro", b =>
                {
                    b.Navigation("CarroManobristas");
                });

            modelBuilder.Entity("Manobra.Domain.Entities.Manobrista", b =>
                {
                    b.Navigation("CarroManobristas");
                });
#pragma warning restore 612, 618
        }
    }
}
