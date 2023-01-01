using System.ComponentModel.DataAnnotations;

namespace Client.Areas.Api.DTOs;

public static class DangNhap
{
	public class DangNhapDTO
	{
		[Required(AllowEmptyStrings = false)]
		public string Email { get; set; } = string.Empty;
		[Required(AllowEmptyStrings = false)]
		public string Password { get; set; } = string.Empty;
	}
}