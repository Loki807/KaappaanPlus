using KaappaanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Configuration
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.AlertTypeRef)
                .WithMany()
                .HasForeignKey(a => a.AlertTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Citizen)
                .WithMany()
                .HasForeignKey(a => a.CitizenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Tenant)
                .WithMany()
                .HasForeignKey(a => a.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
