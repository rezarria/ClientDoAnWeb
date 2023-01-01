using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Api.Controllers;

[Area("Api")]
[Route("/[area]/[controller]/")]
public class TaiKhoan : ControllerBase
{

	private readonly UserManager<Models.XacThucPhanQuyen.TaiKhoan> _userManager;
	private readonly ILogger _logger;

	public TaiKhoan(UserManager<Models.XacThucPhanQuyen.TaiKhoan> userManager, ILogger<TaiKhoan> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	[HttpPost]
	[Route("dangkytructiep")]
	public async Task<IActionResult> DangKyTrucTiep([FromBody] DTOs.DangNhap.DangNhapDto dto, string? returnUrl = null)
	{
		returnUrl = returnUrl ?? Url.Content("~/");

		if (ModelState.IsValid)
		{
			var user = new Models.XacThucPhanQuyen.TaiKhoan { UserName = dto.Email, Email = dto.Email };
			var result = await _userManager.CreateAsync(user, dto.Password);
			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password");
				return Ok();
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}


		}

		return Problem(ModelState.ToString());
	}
}