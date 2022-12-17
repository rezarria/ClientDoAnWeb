namespace Api.ThietLap;

public static class ThietLap
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    public static void Cors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("ToanBo",
				policy =>
				{
					policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowCredentials().AllowAnyOrigin();
				});
		});
	}
}