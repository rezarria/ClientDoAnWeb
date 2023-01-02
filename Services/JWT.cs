using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Client.Models.XacThucPhanQuyen;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Client.Services;

public interface ITokenService
{
	Task<string> TaoToken(string key, string issuer, TaiKhoan user);
	bool KiemTraToken(string key, string issuer, string token);
	ClaimsPrincipal GiaiMaToken(string key, string issuer, string token);
}

public class TokenService : ITokenService
{
	private const double ExpiryDurationMinutes = 30;

	private readonly UserManager<TaiKhoan> _userManager;

	public TokenService(UserManager<TaiKhoan> userManager)
	{
		_userManager = userManager;
	}

	public async Task<string> TaoToken(string key, string issuer, TaiKhoan user)
	{
		IList<Claim> claims = await _userManager.GetClaimsAsync(user);

		claims.Add(new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()));
		SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
		SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
		JwtSecurityToken tokenDescriptor = new(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(ExpiryDurationMinutes), signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}

	public bool KiemTraToken(string key, string issuer, string token)
	{
		try
		{
			_ = GiaiMaToken(key, issuer, token);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public ClaimsPrincipal GiaiMaToken(string key, string issuer, string token)
	{
		byte[] mySecret = Encoding.UTF8.GetBytes(key);
		SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(mySecret);
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.ValidateToken(token,
										  new TokenValidationParameters
										  {
											  ValidateIssuerSigningKey = true,
											  ValidateIssuer = true,
											  ValidateAudience = true,
											  ValidIssuer = issuer,
											  ValidateLifetime = false,
											  ValidAudience = issuer,
											  IssuerSigningKey = mySecurityKey
										  }, out SecurityToken _);
	}
}