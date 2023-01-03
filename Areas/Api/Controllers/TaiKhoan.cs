using Client.Areas.Api.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Api.Controllers;

[Area("Api")]
[Route("/[area]/[controller]/")]
public class TaiKhoan : ControllerBase
{
	private readonly ILogger _logger;

	private readonly UserManager<Models.XacThucPhanQuyen.TaiKhoan> _userManager;

	public TaiKhoan(UserManager<Models.XacThucPhanQuyen.TaiKhoan> userManager, ILogger<TaiKhoan> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	[HttpPost]
	[Route("dangkytructiep")]
	public async Task<IActionResult> DangKyTrucTiep([FromBody] DangNhap.DangNhapDto dto, string? returnUrl = null)
	{
		returnUrl = returnUrl ?? Url.Content("~/");

		if (ModelState.IsValid)
		{
			Models.XacThucPhanQuyen.TaiKhoan user = new()
													{ UserName = dto.Email, Email = dto.Email };
			IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password");
				return Redirect(returnUrl);
			}
			foreach (IdentityError error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);
		}

		return Problem(ModelState.ToString());
	}
}