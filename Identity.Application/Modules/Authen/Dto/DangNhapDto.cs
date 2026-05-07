using AutoMapper;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Modules.Authen.Dto
{
    public class DangNhapDto
    {
        public string tai_khoan { get; set; }
        public string mat_khau { get; set; }
    }
    public class ResultDangNhap
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
    }
    public class DangNhapProfile:Profile
    {
        public DangNhapProfile()
        {
            CreateMap<nguoi_dung, DangNhapDto>();
            CreateMap<DangNhapDto, nguoi_dung>();
        }
    }
}
