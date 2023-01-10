using Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Api.Controllers;

[Area("Api")]
[Route("[area]/[controller]")]
public class TestController : ControllerBase
{

	private readonly CauNoiApiNguon _cauNoi;

	public TestController(CauNoiApiNguon cauNoi)
	{
		_cauNoi = cauNoi;
	}

	[HttpGet]
	public async Task<IActionResult> Do()
	{
		return Ok(await _cauNoi.LayTokenTheoEmailAsync("nam@nam.com", "123", HttpContext.RequestAborted));
	}
}