using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Controllers.XacThuc;

[Area("Admin")]
public class DangNhapController : Controller
{
	public IActionResult DangNhapTrucTiep()
	{
		return View();
	}
}