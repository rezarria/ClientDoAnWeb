using Azure.Core;

namespace Client.Middlewares;

public class XacThucMiddleware
{
	private readonly RequestDelegate _next;

	public XacThucMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var token = context.Session.GetString("Token");
		if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
			context.Request.Headers.Add("Authorization", "Bearer " + token);
		await _next(context);
	}
}

public static class XacThucMiddlewareExtensions
{
	public static IApplicationBuilder UseXacThuc(
		this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<XacThucMiddleware>();
	}
}