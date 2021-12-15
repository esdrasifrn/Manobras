using Manobra.Domain.Entities;
using Manobra.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Infra.Data
{
    public class ManobraContext : DbContext
    {
        public ManobraContext(DbContextOptions<ManobraContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Carro> Carros { get; set; }
        public DbSet<Manobrista> Manobristas { get; set; }
        public DbSet<CarroManobrista> CarroManobristas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carro>().ToTable("Carro");
            modelBuilder.Entity<Manobrista>().ToTable("Monobrista");
            modelBuilder.Entity<CarroManobrista>().ToTable("CarroManobrista");

            modelBuilder.ApplyConfiguration(new CarroMap());
            modelBuilder.ApplyConfiguration(new ManobristaMap());
            modelBuilder.ApplyConfiguration(new CarroManobristaMap());
        }
    }
}
