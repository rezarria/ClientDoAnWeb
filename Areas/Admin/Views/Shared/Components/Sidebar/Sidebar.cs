#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Views.Shared.Components.Sidebar;

public class Sidebar : ViewComponent
{
	public IViewComponentResult Invoke(dynamic data)
	{
		return View();
	}
}