using AutoMapper;
using Azure.Core;
using Identity.Application.Modules.Authen.Dto;
using Identity.Domain.Entities;
using Identity.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedApi.Config;
using SharedApplication.BaseHandler.Command;
using SharedApplication.BaseHandler.Handler;
using SharedApplication.Current;
using SharedApplication.Exceptions;
using SharedApplication.Response;
using SharedApplication.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Modules.Authen.Command
{
    public record DangNhapCommand:CreateCommand<DangNhapDto>,IRequest<ResponseDto<ResultDangNhap>>
    {
    }
    public class DangNhapCommandHandler : BaseCommandHanlder<IdentityDbContext, nguoi_dung>, IRequestHandler<DangNhapCommand, ResponseDto<ResultDangNhap>>
    {
        private readonly IConfiguration _config;
        private readonly JwtOptions _jwt;
        private readonly ICurrentUser _currentUser;

        public DangNhapCommandHandler(IdentityDbContext context, IMapper mapper, 
            IMediator mediator, IConfiguration config, IOptions<JwtOptions> jwtOptions, ICurrentUser currentUser) : base(context, mapper, mediator)
        {
            _config = config;
            _jwt = jwtOptions.Value;
            _currentUser = currentUser;
        }

        public async Task<ResponseDto<ResultDangNhap>> Handle(DangNhapCommand request, CancellationToken cancellationToken)
        {
                var accessTokenExpires = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);
                var expiresInSeconds = (int)((DateTimeOffset)accessTokenExpires).ToUnixTimeSeconds();
                var expires = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);
                const int maxLoginFail = 5;
                const int lockMinutes = 30;
                var user = _context.Set<nguoi_dung>().Where(x =>
                    x.tai_khoan == request.data.tai_khoan)
                .FirstOrDefault();
                if (user == null)
                    throw new AppException(System.Net.HttpStatusCode.BadRequest, "Thông tin tài khoản không chính xác");

                // Nếu hết thời gian khóa thì reset
                if (user.khoa_den_ngay.HasValue && user.khoa_den_ngay < DateTime.Now)
                {
                    user.dang_nhap_that_bai = 0;
                    user.khoa_den_ngay = null;
                }

                // Nếu đang bị khóa
                if (user.khoa_den_ngay.HasValue && user.khoa_den_ngay > DateTime.Now)
                {
                    throw new AppException(
                        HttpStatusCode.BadRequest,
                        $"Tài khoản bị khóa đến {user.khoa_den_ngay:dd/MM/yyyy HH:mm}"
                    );
                }

                bool valid = PasswordHelper.Verify(request.data.mat_khau, user.mat_khau);
                if (!valid)
                {
                    user.dang_nhap_that_bai++;

                    if (user.dang_nhap_that_bai >= maxLoginFail)
                    {
                        user.khoa_den_ngay = DateTime.Now.AddMinutes(lockMinutes);
                        _context.Update(user);
                        _context.SaveChanges();

                        throw new AppException(
                            HttpStatusCode.BadRequest,
                            $"Bạn đã nhập sai quá {maxLoginFail} lần. Tài khoản bị khóa {lockMinutes} phút"
                        );
                    }

                    int remain = maxLoginFail - user.dang_nhap_that_bai;

                    _context.Update(user);
                    _context.SaveChanges();

                    throw new AppException(
                        HttpStatusCode.BadRequest,
                        $"Mật khẩu không đúng, bạn còn {remain} lần"
                    );
                }

                user.dang_nhap_that_bai = 0;
                user.khoa_den_ngay = null;
                _context.Set<nguoi_dung>().Update(user);
                _context.SaveChanges();

                var refresh_token = GenerateRefreshToken.GenerateRefresh_Token();
                var accessToken = GenerateAccessToken(user, accessTokenExpires);
                SaveToken(refresh_token, user);
                return new ResponseDto<ResultDangNhap>
                {
                    data = new ResultDangNhap
                    {
                        access_token = accessToken, 
                        refresh_token = refresh_token,
                        expires_in = expiresInSeconds
                    }
                };
        }
        private string GenerateAccessToken(nguoi_dung nguoi_dung, DateTime expires)
        {
            try
            {
                var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.UniqueName, nguoi_dung.tai_khoan),
                new Claim(ClaimTypes.NameIdentifier, nguoi_dung.id.ToString()),
                new Claim(ClaimTypes.Role,"Admin")
                };
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwt.SecretKey)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void SaveToken(string token, nguoi_dung nguoidung)
        {
            try
            {


                var refreshExpires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays);
                var item = _context.Set<nguoi_dung_refresh_token>().Where(x => x.nguoi_dung_id == nguoidung.id && !x.da_thu_hoi &&x.ngay_het_han > DateTime.UtcNow).FirstOrDefault();
                if (item == null)
                {
                    _context.Set<nguoi_dung_refresh_token>().Add(new nguoi_dung_refresh_token
                    {
                        refresh_token = token,
                        nguoi_dung_id = nguoidung.id,
                        ngay_het_han = refreshExpires,
                        ip_tao = _currentUser.ip_address,
                        thiet_bi_dang_nhap= _currentUser.Browser
                    });
                }
                else
                {
                    item.refresh_token = token;
                    item.ip_tao = _currentUser.ip_address;
                    item.thiet_bi_dang_nhap = _currentUser.Browser;
                    _context.Set<nguoi_dung_refresh_token>().Update(item);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
    }
}
