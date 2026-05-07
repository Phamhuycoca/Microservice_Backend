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
    public class NguoiDungRefreshTokenConfiguration : IEntityTypeConfiguration<nguoi_dung_refresh_token>
    {
        public void Configure(EntityTypeBuilder<nguoi_dung_refresh_token> builder)
        {
            builder.ToTable("nguoi_dung_refresh_token");
            builder.HasKey(x => new { x.nguoi_dung_id});
            builder.HasOne(x => x.nguoi_dung)
               .WithMany(x => x.lst_nguoi_dung_refresh_token)
               .HasForeignKey(x => x.nguoi_dung_id)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
