namespace Client.ThietLap;

public partial class ThietLap
{
	/// <summary>
	/// </summary>
	/// <param name="services"></param>
	public static void ThietLapSession(this IServiceCollection services)
	{
		services.AddSession();
	}
}