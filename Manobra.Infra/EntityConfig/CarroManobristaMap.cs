using Manobra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Infra.EntityConfig
{
    public class CarroManobristaMap : IEntityTypeConfiguration<CarroManobrista>
    {
        public void Configure(EntityTypeBuilder<CarroManobrista> builder)
        {
            builder.HasKey(ug => new { ug.CarroId, ug.ManobristaId });

            builder.HasOne(ug => ug.CarroManobrado)
                .WithMany(u => u.CarroManobristas)
                .HasForeignKey(ug => ug.CarroId);

            builder.HasOne(ug => ug.ResponsavelPelaManobra)
                .WithMany(g => g.CarroManobristas)
                .HasForeignKey(ug => ug.ManobristaId);
        }
    }
}
