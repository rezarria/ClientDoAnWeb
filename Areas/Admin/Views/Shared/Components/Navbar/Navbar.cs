#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Views.Shared.Components.Navbar;

public class Navbar : ViewComponent
{
	public IViewComponentResult Invoke(dynamic data)
	{
		return View();
	}
}