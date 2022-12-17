#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Controllers;

[Area("Admin")]
public class HocPhanController : Controller
{
	public IActionResult Index() => View();
}