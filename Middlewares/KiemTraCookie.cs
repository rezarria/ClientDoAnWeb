using Microsoft.Extensions.Primitives;

namespace Client.Middlewares;

public class KiemTraCookie
{
	private readonly RequestDelegate _next;

	public KiemTraCookie(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.ContainsKey("Authorization") &&
		    context.Request.Cookies.ContainsKey("jwt") &&
		    context.Request.Cookies.TryGetValue("jwt", out string? token))
			context.Request.Headers.Authorization = new StringValues("Bearer " + token);

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