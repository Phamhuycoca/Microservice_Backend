using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class nguoi_dung_refresh_token
    {
        public Guid nguoi_dung_id { get; set; }

        public string refresh_token { get; set; }

        public DateTime ngay_het_han { get; set; }

        public bool da_thu_hoi { get; set; } = false;

        public DateTime? ngay_thu_hoi { get; set; }

        public string? ip_tao { get; set; }

        public string? ip_thu_hoi { get; set; }

        public string? thiet_bi_dang_nhap { get; set; }
        public string? thiet_bi_dang_xuat { get; set; }

        public bool da_het_han
            => DateTime.UtcNow >= ngay_het_han;

        public bool con_hieu_luc
            => !da_thu_hoi && !da_het_han;
        public virtual nguoi_dung nguoi_dung { get; set; }
    }
}
