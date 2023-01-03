using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Client.Areas.Api.DTOs.DangNhap;

namespace Client.Areas.Api.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class DangNhapController : ControllerBase
{
	private readonly IConfiguration _configuration;
	private readonly ITokenService _tokenService;
	private readonly UserManager<Models.XacThucPhanQuyen.TaiKhoan> _userManager;

	public DangNhapController(
		UserManager<Models.XacThucPhanQuyen.TaiKhoan> userManager,
		ITokenService tokenService,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_tokenService = tokenService;
		_configuration = configuration;
	}

	[HttpPost]
	[Route("tructiep")]
	public async Task<IActionResult> TrucTiep([FromBody] DangNhapDto dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		Models.XacThucPhanQuyen.TaiKhoan? user = await _userManager.FindByEmailAsync(dto.Email);

		if (user is null)
			return NotFound();
		if (!await _userManager.CheckPasswordAsync(user, dto.Password))
			return BadRequest("Sai thông tin đăng nhập");

		string key = _configuration["jwt:Key"] ?? throw new Exception();
		string issur = _configuration["Jwt:Issuer"] ?? throw new Exception();
		string token = await _tokenService.TaoToken(key, issur, user);

		HttpContext.Session.SetString("Token", token);

		return Ok(new
				  {
					  jwt = token
				  });
	}

	[HttpGet]
	[Authorize]
	public string Check()
	{
		return string.Join("\n", User.Claims.Select(x => $"{x.Type}:{x.Value.ToString()}"));
	}
}