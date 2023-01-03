using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Client.Areas.Api.DTOs.DangNhap;

namespace Client.Areas.Api.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class DangNhapController : ControllerBase
{
	private readonly ICauNoiApiNguon _cauNoiApiNguon;
	private readonly IQuanLyTaiKhoan _quanLyTaiKhoan;

	public DangNhapController(
		IQuanLyTaiKhoan quanLyTaiKhoan, ICauNoiApiNguon cauNoiApiNguon)
	{
		_quanLyTaiKhoan = quanLyTaiKhoan;
		_cauNoiApiNguon = cauNoiApiNguon;
	}

	[HttpPost]
	[Route("tructiep")]
	public async Task<IActionResult> TrucTiep([FromBody] DangNhapDto dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		if (await _quanLyTaiKhoan.KiemTraEmailAsync(dto.Email))
			return NotFound();
		if (!await _quanLyTaiKhoan.XacThucEmailAsync(dto.Email, dto.Password, HttpContext.RequestAborted))
			return BadRequest("Sai thông tin đăng nhập");

		string token = await _quanLyTaiKhoan.DangNhapEmailAsync(dto.Email, HttpContext.RequestAborted);

		string apiToken = await _cauNoiApiNguon.LayTokenTheoEmailAsync(dto.Email, dto.Password, HttpContext.RequestAborted);

		if (string.IsNullOrEmpty(apiToken)) return NoContent();

		return Ok(new
				  {
					  jwt = token,
					  api = apiToken
				  });
	}

	[HttpGet]
	[Authorize]
	public string Check()
	{
		return string.Join("\n", User.Claims.Select(x => $"{x.Type}:{x.Value.ToString()}"));
	}
}