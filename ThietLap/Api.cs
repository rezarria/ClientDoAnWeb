namespace Client.ThietLap;

/// <summary>
/// </summary>
public static partial class ThietLap
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    public static void ThietLapApiMayChu(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MayChuApi>(
             builder.Configuration.GetSection("MayChuApi")
         );

    }

}

public class MayChuApi
{
    public string DiaChi { get; init; } = String.Empty;
}