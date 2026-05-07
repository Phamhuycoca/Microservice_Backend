using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Configuration
{
    public class NguoiDungConfiguration : IEntityTypeConfiguration<nguoi_dung>
    {
        public void Configure(EntityTypeBuilder<nguoi_dung> builder)
        {
            builder.ToTable("nguoi_dung");
            builder.Property(x => x.tai_khoan)
               .IsRequired()
               .HasMaxLength(100);
            builder.HasMany<nguoi_dung_refresh_token>()
                .WithOne(x => x.nguoi_dung)
                .HasForeignKey(x => x.nguoi_dung_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
