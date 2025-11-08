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
    public class AlertResponderConfiguration : IEntityTypeConfiguration<AlertResponder>
    {
        public void Configure(EntityTypeBuilder<AlertResponder> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Alert)
                   .WithMany(a => a.Responders)
                   .HasForeignKey(a => a.AlertId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.AppUser)
                   .WithMany()
                   .HasForeignKey(a => a.AppUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
