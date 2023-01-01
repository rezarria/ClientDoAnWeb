using Client.Models.XacThucPhanQuyen;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Client.Contexts;

public class XacThucContext : IdentityDbContext<TaiKhoan, QuyenHan, Guid>
{
	public XacThucContext(DbContextOptions<XacThucContext> options) : base(options)
	{
	}
}