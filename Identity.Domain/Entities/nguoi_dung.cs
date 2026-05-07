using SharedDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities;

public class nguoi_dung : AuditableBaseEntity
{
    public nguoi_dung():base(){
        this.lst_nguoi_dung_refresh_token = new HashSet<nguoi_dung_refresh_token>();
    }
    public string? ho { get; set; }
    public string? ten { get; set; }
    public string? ten_day_du { get; set; }
    public string tai_khoan { get; set; }
    public string mat_khau { get; set; }
    public string? email { get; set; }
    public string? so_dien_thoai { get; set; }
    public int? gioi_tinh { get; set; }
    public string? anh_dai_dien { get; set; }
    public bool trang_thai { get; set; } = true;
    public int dang_nhap_that_bai { get; set; } = 0;
    public bool khoa_dang_nhap { get; set; } = false;
    public DateTime? khoa_den_ngay { get; set; }
    public string security_stamp { get; set; }= Guid.NewGuid().ToString();
    public string concurrency_stamp { get; set; } = Guid.NewGuid().ToString();
    public virtual ICollection<nguoi_dung_refresh_token> lst_nguoi_dung_refresh_token { get; set; }
}
