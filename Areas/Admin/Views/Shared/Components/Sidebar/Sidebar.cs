#region

using Client.Areas.Admin.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Client.Areas.Admin.Views.Shared.Components.Sidebar;

public class Sidebar : ViewComponent
{

	private ClientDbContext _context;

	public Sidebar(ClientDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync(dynamic _)
	{
		List<Models.SidebarNavItem> data = await (
			from x in _context.SidebarNavItem
			where x.ParentId == null
			select x
		)
		.Include(x => x.Childs)
		.AsNoTracking()
		.ToListAsync(HttpContext.RequestAborted);

		IQueryable<Models.SidebarNavItem> targets = data.SelectMany(x => x.Childs).AsQueryable();
		List<Task> jobs = new List<Task>();
		while (targets.Any())
		{
			await targets.ForEachAsync(x =>
			{
				jobs.Add(_context.Entry(x).Collection(y => y.Childs).LoadAsync(HttpContext.RequestAborted));
			});
			Task.WaitAll(jobs.ToArray(), HttpContext.RequestAborted);
			targets = targets.SelectMany(x => x.Childs).AsQueryable();
		}
		return View(data);
	}
}