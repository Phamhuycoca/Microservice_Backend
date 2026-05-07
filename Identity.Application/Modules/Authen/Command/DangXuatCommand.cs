using AutoMapper;
using Identity.Domain.Entities;
using Identity.Infrastructure.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedApplication.BaseHandler.Handler;
using SharedApplication.Current;
using SharedApplication.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Modules.Authen.Command
{
    public record DangXuatCommand:IRequest<ResponseDto<bool>>
    {
    }
    public class DangXuatCommandHandler : BaseCommandHanlder<IdentityDbContext, nguoi_dung>, IRequestHandler<DangXuatCommand, ResponseDto<bool>>
    {
        private readonly ICurrentUser _currentUser;
        public DangXuatCommandHandler(IdentityDbContext context, IMapper mapper, IMediator mediator, ICurrentUser currentUser) : base(context, mapper, mediator)
        {
            _currentUser = currentUser;
        }

        public async Task<ResponseDto<bool>> Handle(DangXuatCommand request,CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto<bool>
                {
                    success = false,
                    message = "Unauthorized",
                    statusCode=401,
                    data=false
                };
            }

            var item = await _context
                .Set<nguoi_dung_refresh_token>()
                .FirstOrDefaultAsync(x =>
                    x.nguoi_dung_id == Guid.Parse(userId) &&
                    !x.da_thu_hoi &&
                    x.ngay_het_han > DateTime.UtcNow,
                    cancellationToken);

            if (item != null)
            {
                item.da_thu_hoi = true;
                item.thiet_bi_dang_xuat = _currentUser.Browser;
                item.ngay_thu_hoi = DateTime.UtcNow;
                _context.Update(item);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new ResponseDto<bool>
            {
                data = true,
                message = "Đăng xuất thành công",
                success = true,
                statusCode = 200
            };
        }
    }
}
