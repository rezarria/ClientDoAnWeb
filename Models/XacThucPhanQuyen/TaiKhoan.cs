using Microsoft.AspNetCore.Identity;

namespace Client.Models.XacThucPhanQuyen;

public class TaiKhoan : IdentityUser<Guid>
{
	public Guid? IdApi { get; set; }
}