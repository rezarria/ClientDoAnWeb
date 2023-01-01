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
			where x.ParentId == null && x.Active == true
			select x
		)
		.Include(x => x.Childs)
		.OrderBy(x => x.Order)
		.ToListAsync(HttpContext.RequestAborted);

		data.ForEach(x =>
		{
			x.Childs.RemoveAll(y => y.Active == false);
			x.Childs.Sort((a, b) => a.Order - b.Order);
		});

		List<Models.SidebarNavItem> targets = data.SelectMany(x => x.Childs).ToList();
		List<Task> jobs = new List<Task>();
		while (targets.Any())
		{
			targets.ForEach(x =>
			{
				jobs.Add(_context.Entry(x).Collection(y => y.Childs).LoadAsync(HttpContext.RequestAborted));
			});
			Task.WaitAll(jobs.ToArray(), HttpContext.RequestAborted);
			targets.ForEach(x =>
			{
				x.Childs.RemoveAll(y => y.Active == false);
				x.Childs.Sort((a, b) => a.Order - b.Order);
			});
			targets = targets.SelectMany(x => x.Childs).ToList();
		}
		return View(data);
	}
}