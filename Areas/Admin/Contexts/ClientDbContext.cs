#region

using Client.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Client.Areas.Admin.Contexts;

public class ClientDbContext : DbContext
{
	public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
	{
	}

	public DbSet<SidebarNavItem> SidebarNavItem { get; set; } = null!;
}