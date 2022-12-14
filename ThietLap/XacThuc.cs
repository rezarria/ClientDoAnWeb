#region

using Client.Contexts;
using Client.Models.XacThucPhanQuyen;
using Client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RezUtility.Contexts;
using RezUtility.Services;
using System.Text;

#endregion

namespace Client.ThietLap;

/// <summary>
/// </summary>
public static partial class ThietLap
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static void ThietLapXacThuc(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		ConfigurationManager configuration = builder.Configuration;

		services.AddDbContext<IXacThucDbContext, XacThucContext>(options => options.UseSqlite(configuration.GetConnectionString("XacThuc")));

		services
			.AddTransient<IQuanLyTaiKhoan, QuanLyTaiKhoan>()
			.AddIdentity<TaiKhoan, QuyenHan>()
			.AddEntityFrameworkStores<XacThucContext>()
			.AddDefaultTokenProviders();
		services
			.AddTokenService()
			.AddAuthentication(options =>
							   {
								   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
								   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
								   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
							   })
			.AddJwtBearer(options =>
						  {
							  string issuer = configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer không tồn tại");
							  string key = configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key không tồn tại");
							  options.TokenValidationParameters = new TokenValidationParameters
																  {
																	  ValidateIssuer = true,
																	  ValidateAudience = true,
																	  ValidateLifetime = true,
																	  ValidateIssuerSigningKey = true,
																	  ValidIssuer = issuer,
																	  ValidAudience = issuer,
																	  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
																  };
						  });

		builder.Services.AddAuthorization();

		builder.Services.Configure<IdentityOptions>(options =>
												    {
													    options.Password.RequireDigit = false;
													    options.Password.RequireLowercase = false;
													    options.Password.RequireNonAlphanumeric = false;
													    options.Password.RequireUppercase = false;
													    options.Password.RequiredLength = 6;
													    options.Password.RequiredUniqueChars = 0;
													    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
													    options.Lockout.MaxFailedAccessAttempts = 5;
													    options.Lockout.AllowedForNewUsers = true;
													    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
													    options.User.RequireUniqueEmail = false;
												    });

		services.ConfigureApplicationCookie(options =>
										    {
											    // options.Cookie.HttpOnly = true;  
											    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
										    });


		services.Configure<SecurityStampValidatorOptions>(options =>
														  {
															  options.ValidationInterval = TimeSpan.FromSeconds(5);
														  });

		services.Configure<TokenServiceOptions>(options =>
											    {
												    options.ExpiryDurationMinutes = 30;
												    options.Key = configuration["Jwt:Key"] ?? throw new Exception();
												    options.Issuer = configuration["Jwt:Issuer"] ?? throw new Exception();
											    });
	}
}