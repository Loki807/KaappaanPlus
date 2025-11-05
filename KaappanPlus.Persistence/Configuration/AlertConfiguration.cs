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

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.NoAction); // 👈 prevent cascade chain

            builder.Property(a => a.Type).HasMaxLength(100);
        }
    }
}
