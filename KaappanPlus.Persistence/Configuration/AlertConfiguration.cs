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

            builder.Property(a => a.AlertType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(a => a.Location)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(30);

            // Relationships
            builder.HasOne(a => a.Citizen)
                .WithMany()
                .HasForeignKey(a => a.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Tenant)
                .WithMany()
                .HasForeignKey(a => a.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Alerts");
        }
    }
}
