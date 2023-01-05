using Client.Models.XacThucPhanQuyen;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RezUtility.Contexts;
using RezUtility.Models;

namespace Client.Contexts;

public class XacThucContext : IdentityDbContext<TaiKhoan, QuyenHan, Guid>, IXacThucDbContext
{
	public XacThucContext(DbContextOptions<XacThucContext> options) : base(options)
	{
	}

	public DbSet<TokenDangXuat> TokenDangXuat { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<TokenDangXuat>(entity =>
									  {
										  entity.Property(x => x.Token).ValueGeneratedNever();
										  entity.Property(x => x.Exp).HasConversion(new TimeSpanToTicksConverter());
									  });
	}
}