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
    public class AlertTypeConfiguration : IEntityTypeConfiguration<AlertType>
    {
        public void Configure(EntityTypeBuilder<AlertType> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(a => a.Name).IsUnique();
            builder.Property(a => a.Description).HasMaxLength(200);
            builder.ToTable("AlertTypes");
        }
    }
}
