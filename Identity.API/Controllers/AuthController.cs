using Identity.API.Models;
using Identity.Application.Modules.Authen.Command;
using Identity.Application.Modules.Authen.Dto;
using Identity.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApplication.Exceptions;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Identity.API.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IdentityDbContext _context;
        public AuthController(IMediator mediator,IdentityDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        [HttpGet("dang-nhap")]
        public IActionResult DangNhap()
        {
            return View();
        }
        [HttpPost("dang-nhap")]
        public async Task<IActionResult> DangNhap(DangNhapViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var dto = new DangNhapDto
                {
                    tai_khoan = model.tai_khoan,
                    mat_khau = model.mat_khau
                };

                var result = await _mediator.Send(new DangNhapCommand { data = dto });

                if (!result.success)
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }

                TempData["token"] = result.data.access_token;
                return RedirectToAction(nameof(RedirectToReact));
            }
            catch(AppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }
        [HttpGet("RedirectToReact")]
        public IActionResult RedirectToReact()
        {
            var token = TempData["token"]?.ToString();

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(DangNhap));
            }

            return Redirect(
                $"http://localhost:5173/auth-callback?token={token}");
        }
        [Authorize]
        [HttpPost("dang-xuat")]
        public async Task<IActionResult> DangXuat()
        {
            var result = await _mediator.Send(new DangXuatCommand());

            await HttpContext.SignOutAsync();

            return Ok(result);
        }
    }
}
