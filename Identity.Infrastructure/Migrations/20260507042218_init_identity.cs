using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init_identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "nguoi_dung",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ten_day_du = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tai_khoan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    so_dien_thoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gioi_tinh = table.Column<int>(type: "int", nullable: true),
                    anh_dai_dien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai = table.Column<bool>(type: "bit", nullable: false),
                    dang_nhap_that_bai = table.Column<int>(type: "int", nullable: false),
                    khoa_dang_nhap = table.Column<bool>(type: "bit", nullable: false),
                    khoa_den_ngay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    security_stamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nguoi_dung", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nguoi_dung_refresh_token",
                schema: "dbo",
                columns: table => new
                {
                    nguoi_dung_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_het_han = table.Column<DateTime>(type: "datetime2", nullable: false),
                    da_thu_hoi = table.Column<bool>(type: "bit", nullable: false),
                    ngay_thu_hoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ip_tao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_thu_hoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thiet_bi_dang_nhap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thiet_bi_dang_xuat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nguoi_dung_refresh_token", x => x.nguoi_dung_id);
                    table.ForeignKey(
                        name: "FK_nguoi_dung_refresh_token_nguoi_dung_nguoi_dung_id",
                        column: x => x.nguoi_dung_id,
                        principalSchema: "dbo",
                        principalTable: "nguoi_dung",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nguoi_dung_refresh_token",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "nguoi_dung",
                schema: "dbo");
        }
    }
}
