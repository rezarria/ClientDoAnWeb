using Client.Services;
using Microsoft.Extensions.Primitives;

namespace Client.Middlewares;

public class KiemTraCookie
{
	private readonly ILogger _logger;
	private readonly RequestDelegate _next;
	private readonly IServiceProvider _serviceProvider;
	private readonly ITokenDangXuatService _tokenDangXuat;

	public KiemTraCookie(RequestDelegate next, IServiceProvider serviceProvider, ILogger<KiemTraCookie> logger, ITokenDangXuatService tokenDangXuat)
	{
		_next = next;
		_serviceProvider = serviceProvider;
		_logger = logger;
		_tokenDangXuat = tokenDangXuat;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.ContainsKey("Authorization") &&
		    context.Request.Cookies.ContainsKey("jwt") &&
		    context.Request.Cookies.TryGetValue("jwt", out string token))
		{
			context.Request.Headers.Authorization = new("Bearer " + token);
		}

		await _next(context);
	}
}

public static class KiemTraCookieMiddlewareExtensions
{
	public static IApplicationBuilder UseKiemTraCookie(
		this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<KiemTraCookie>();
	}
}