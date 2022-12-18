namespace Client.ThietLap;

/// <summary>
/// </summary>
public static partial class ThietLap
{
	/// <summary>
	/// </summary>
	public static void ThietLapApiMayChu(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<MayChuApi>(
			builder.Configuration.GetSection("MayChuApi")
		);
	}
}

public class MayChuApi
{
	public string DiaChi { get; init; } = string.Empty;
}