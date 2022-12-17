#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Controllers;

[Area("Admin")]
public class LopHocController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}