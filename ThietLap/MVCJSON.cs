using Newtonsoft.Json;

namespace Client.ThietLap;

/// <summary>
/// </summary>
public static partial class ThietLap
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    public static void ThietLapMVCJSON(this IServiceCollection services)
    {
        services.AddControllersWithViews().AddNewtonsoftJson(x =>
        {
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            x.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            x.SerializerSettings.ConstructorHandling = ConstructorHandling.Default;
            x.SerializerSettings.Formatting = Formatting.Indented;
            x.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Reuse;
        });
    }
}