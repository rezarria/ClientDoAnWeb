#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Controllers;

[Area("Admin")]
public class KieuNguoiDungController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}