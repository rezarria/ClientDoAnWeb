﻿#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Client.Areas.Admin.Controllers;

[Area("Admin")]
public class PhongHoc : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}