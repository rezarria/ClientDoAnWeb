using Client.Contexts;
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
	private readonly IConfiguration _configuration;
	private readonly XacThucContext _context;
	private readonly ITokenDangXuatService _tokenDangXuat;
	private readonly ITokenService _tokenService;

	public DangXuatController(XacThucContext context, ITokenService tokenService, IConfiguration configuration, ITokenDangXuatService tokenDangXuat)
	{
		_context = context;
		_tokenService = tokenService;
		_configuration = configuration;
		_tokenDangXuat = tokenDangXuat;
	}

	[HttpPost]
	public async Task<IActionResult> YeuCau(string token)
	{
		string key = _configuration["Jwt:Key"] ?? throw new Exception();
		string issuer = _configuration["Jwt:Issuer"] ?? throw new Exception();
		try
		{
			ClaimsPrincipal claimsPrincipal = _tokenService.GiaiMaToken(key, issuer, token);
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
		}
		return Ok();
	}
}