#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Views.Shared.Components.ElFinder;

public class ElFinder : ViewComponent
{
	public IViewComponentResult Invoke(string id, string options = "{}")
	{
		ViewData["Options"] = options;
		ViewData["Id"] = id;
		return View();
	}
}