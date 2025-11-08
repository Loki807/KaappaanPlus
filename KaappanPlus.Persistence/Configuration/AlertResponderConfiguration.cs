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
            builder.HasOne(r => r.Alert)
                        .WithMany()
                        .HasForeignKey(r => r.AlertId)
                        .OnDelete(DeleteBehavior.Restrict);
                            ;

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
