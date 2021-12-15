using Manobra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manobra.Infra.EntityConfig
{
    public class ManobristaMap : IEntityTypeConfiguration<Manobrista>
    {
        public void Configure(EntityTypeBuilder<Manobrista> builder)
        {
            builder.HasKey(c => c.ManobristaId);
            builder
             .Property(l => l.ManobristaId).UseIdentityColumn();
        }
    }
}
