#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Views.Shared.Components.ContentHeader;

public class ContentHeader : ViewComponent
{
	public IViewComponentResult Invoke(dynamic data)
	{
		return View();
	}
}