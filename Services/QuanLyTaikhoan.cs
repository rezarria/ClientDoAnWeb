using Client.Contexts;
using Client.Models.XacThucPhanQuyen;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Services;

public interface IQuanLyTaiKhoan
{
	Task<string> DangNhapAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default);
	Task<string> DangNhapUserNameAsync(string email, string matKhau, CancellationToken cancellationToken = default);
	Task<string> DangNhapEmailAsync(string email, CancellationToken cancellationToken = default);
	Task DangKyAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default);
	Task DangKyAsync(TaiKhoan taiKhoan, string matKhau, CancellationToken cancellationToken = default);
	void DangXuat(string token);
	Task<bool> KiemTraUsername(string username, CancellationToken cancellationToken = default);
	Task<bool> KiemTraEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<bool> KiemTraId(Guid id, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimTaiKhoanAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimIdAsync(Guid idTaiKhoan, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimUserNameAsync(string username, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<bool> XacThucAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default);
	Task<bool> XacThucEmailAsync(string dtoEmail, string dtoPassword, CancellationToken httpContextRequestAborted);
}

public class QuanLyTaiKhoan : IQuanLyTaiKhoan
{
	private readonly XacThucContext _context;
	private readonly ITokenDangXuatService _tokenDangXuat;
	private readonly ITokenService _tokenService;
	private readonly UserManager<TaiKhoan> _userManager;

	public QuanLyTaiKhoan(XacThucContext context, ITokenService tokenService, ITokenDangXuatService tokenDangXuat, UserManager<TaiKhoan> userManager)
	{
		_context = context;
		_tokenService = tokenService;
		_tokenDangXuat = tokenDangXuat;
		_userManager = userManager;
	}

	public async Task<string> DangNhapAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		if (!await KiemTraId(idTaiKhoan, cancellationToken) ||
		    !await XacThucAsync(idTaiKhoan, matKhau, cancellationToken))
			return string.Empty;
		List<Claim> claims = await LayClaimIdAsync(idTaiKhoan, cancellationToken);
		return _tokenService.TaoTokenAsync(claims);
	}

	public async Task<string> DangNhapUserNameAsync(string email, string matKhau, CancellationToken cancellationToken = default)
	{
		if (!await KiemTraUsername(email, cancellationToken) ||
		    !await XacThucUserNameAsync(email, matKhau, cancellationToken))
			return string.Empty;
		return await DangNhapEmailAsync(email, cancellationToken);
	}
	public async Task<string> DangNhapEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		List<Claim> claims = await LayClaimUserNameAsync(email, cancellationToken);
		return _tokenService.TaoTokenAsync(claims);
	}

	/// <summary>
	/// </summary>
	/// <param name="taiKhoan"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="Exception"></exception>
	public async Task DangKyAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default)
	{
		if (_context.Users.Any(x => !string.IsNullOrEmpty(x.UserName) && x.UserName.Equals(taiKhoan)))
			throw new Exception("Tài khoản đã tồn tại");
		IdentityResult result = await _userManager.CreateAsync(taiKhoan);
		if (!result.Succeeded)
			throw new Exception("Cập nhật database thất bại");
	}

	/// <summary>
	/// </summary>
	/// <param name="taiKhoan"></param>
	/// <param name="matKhau"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="Exception"></exception>
	public async Task DangKyAsync(TaiKhoan taiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		if (_context.Users.Any(x => !string.IsNullOrEmpty(x.UserName) && x.UserName.Equals(taiKhoan)))
			throw new Exception("Tài khoản đã tồn tại");
		IdentityResult result = await _userManager.CreateAsync(taiKhoan, matKhau);
		if (!result.Succeeded)
			throw new Exception("Cập nhật database thất bại");
	}

	public void DangXuat(string token)
	{
		ClaimsPrincipal claimsPrincipal = _tokenService.GiaiMaToken(token);
		TokenDangXuat tokenDangXuat = new()
									  {
										  Token = token,
										  Exp = TimeSpan.FromSeconds(double.Parse(claimsPrincipal.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Exp)).Value))
									  };
		_tokenDangXuat.ThemToken(tokenDangXuat);
	}


	public async Task<bool> KiemTraUsername(string username, CancellationToken cancellationToken = default)
	{
		return await _userManager.Users.AnyAsync(predicate: x => !string.IsNullOrEmpty(x.UserName) && x.UserName.Equals(username), cancellationToken);
	}
	public Task<bool> KiemTraEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		return _userManager.Users.AnyAsync(predicate: x => !string.IsNullOrEmpty(x.Email) && x.Email.Equals(email), cancellationToken);
	}

	public async Task<bool> KiemTraId(Guid id, CancellationToken cancellationToken = default)
	{
		return await _userManager.Users.AnyAsync(predicate: x => x.Id == id, cancellationToken);
	}

	public async Task<List<Claim>> LayClaimTaiKhoanAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default)
	{
		return (await _userManager.GetClaimsAsync(taiKhoan)).ToList();
	}

	public async Task<List<Claim>> LayClaimIdAsync(Guid idTaiKhoan, CancellationToken cancellationToken = default)
	{
		TaiKhoan? taiKhoan = await _userManager.Users.FirstOrDefaultAsync(predicate: x => x.Id.Equals(idTaiKhoan), cancellationToken);
		return taiKhoan is null ? new List<Claim>() : (await _userManager.GetClaimsAsync(taiKhoan)).ToList();
	}

	public async Task<List<Claim>> LayClaimUserNameAsync(string username, CancellationToken cancellationToken = default)
	{
		return await LayClaim(username, _userManager.FindByEmailAsync, cancellationToken);
	}

	public async Task<List<Claim>> LayClaimEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		return await LayClaim(email, _userManager.FindByEmailAsync, cancellationToken);
	}

	public async Task<bool> XacThucAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		TaiKhoan? taiKhoan = await _userManager.Users.FirstOrDefaultAsync(predicate: x => x.Id.Equals(idTaiKhoan), cancellationToken);
		if (taiKhoan is null) return false;
		return await _userManager.CheckPasswordAsync(taiKhoan, matKhau);
	}

	public async Task<bool> XacThucEmailAsync(string email, string matKhau, CancellationToken cancellationToken = default)
	{
		return await XacThuc(email, matKhau, _userManager.FindByEmailAsync, cancellationToken);
	}

	public async Task<string> DangNhapEmailAsync(string email, string matKhau, CancellationToken cancellationToken = default)
	{
		if (!await KiemTraEmailAsync(email, cancellationToken) ||
		    !await XacThucEmailAsync(email, matKhau, cancellationToken))
			return string.Empty;
		List<Claim> claims = await LayClaimEmailAsync(email, cancellationToken);
		return _tokenService.TaoTokenAsync(claims);
	}

	private async Task<List<Claim>> LayClaim(string content, Func<string, Task<TaiKhoan?>> method, CancellationToken cancellationToken = default)
	{
		TaiKhoan? taiKhoan = await method(content);
		if (taiKhoan is null) return new List<Claim>();
		Task task = _userManager.GetClaimsAsync(taiKhoan);
		await task.WaitAsync(cancellationToken);
		cancellationToken.ThrowIfCancellationRequested();
		IList<Claim> list = await _userManager.GetClaimsAsync(taiKhoan);
		return list.ToList();
	}

	public async Task<bool> XacThucUserNameAsync(string username, string matKhau, CancellationToken cancellationToken = default)
	{
		return await XacThuc(username, matKhau, _userManager.FindByNameAsync, cancellationToken);
	}

	private async Task<bool> XacThuc(string content, string matKhau, Func<string, Task<TaiKhoan?>> method, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		TaiKhoan? user = await method(content);
		if (user is null) return false;
		return await _userManager.CheckPasswordAsync(user, matKhau);
	}
}