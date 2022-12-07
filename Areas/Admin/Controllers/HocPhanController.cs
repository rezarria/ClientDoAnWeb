using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Controllers;

[Area("Admin")]
public class HocPhanController : Controller
{
	public IActionResult Index() => View();
}