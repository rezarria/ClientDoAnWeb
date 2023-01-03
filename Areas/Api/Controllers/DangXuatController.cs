using Client.Models.XacThucPhanQuyen;
using Client.Services;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Client.Areas.Api.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class DangXuatController : ControllerBase
{
	private readonly ITokenDangXuatService _tokenDangXuat;
	private readonly ITokenService _tokenService;

	public DangXuatController(ITokenService tokenService, ITokenDangXuatService tokenDangXuat)
	{
		_tokenService = tokenService;
		_tokenDangXuat = tokenDangXuat;
	}

	[HttpPost]
	public IActionResult YeuCau(string token)
	{
		try
		{
			ClaimsPrincipal claimsPrincipal = _tokenService.GiaiMaToken(token);
			Claim? expiration = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Expiration);
			if (expiration is not null)
			{
				TokenDangXuat tokenDangXuat = new()
											  {
												  Token = token,
												  Exp = TimeSpan.FromSeconds(int.Parse(expiration.Value))
											  };
				_tokenDangXuat.ThemToken(tokenDangXuat);
			}
		}
		catch (Exception)
		{
			return NotFound();
		}
		return Ok();
	}
}