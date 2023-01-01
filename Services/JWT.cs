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
	bool kiemTraToken(string key, string issuer, string token);
}

public class TokenService : ITokenService
{
	private const double EXPIRY_DURATION_MINUTES = 30;

	private readonly UserManager<TaiKhoan> _userManager;

	public TokenService(UserManager<TaiKhoan> userManager)
	{
		_userManager = userManager;
	}

	public async Task<string> TaoToken(string key, string issuer, TaiKhoan user)
	{
		var claims = await _userManager.GetClaimsAsync(user);

		claims.Add(new Claim("role1", "true"));

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
		var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}
	public bool kiemTraToken(string key, string issuer, string token)
	{
		var mySecret = Encoding.UTF8.GetBytes(key);
		var mySecurityKey = new SymmetricSecurityKey(mySecret);
		var tokenHandler = new JwtSecurityTokenHandler();
		try
		{
			tokenHandler.ValidateToken(token,
			new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = issuer,
				ValidAudience = issuer,
				IssuerSigningKey = mySecurityKey,
			}, out SecurityToken validatedToken);
		}
		catch
		{
			return false;
		}
		return true;
	}
}
