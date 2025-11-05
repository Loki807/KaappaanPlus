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
            builder.HasKey(ar => ar.Id);

            // ✅ Map Alert → AlertResponders
            builder.HasOne(ar => ar.Alert)
                   .WithMany(a => a.Responders)
                   .HasForeignKey(ar => ar.AlertId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ✅ Map AppUser → AlertResponders
            builder.HasOne(ar => ar.Responder)
                   .WithMany()
                   .HasForeignKey(ar => ar.ResponderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(ar => ar.ResponseStatus)
                   .HasMaxLength(50);
        }
    }
}
