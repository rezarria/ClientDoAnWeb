using System.IO.Compression;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;

namespace Client.ThietLap;

public static partial class ThietLap
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="services"></param>
	public static void WebMarkupMin(this IServiceCollection services)
	{
		services.AddResponseCaching();
		services.AddWebMarkupMin(options =>
		{
			options.AllowMinificationInDevelopmentEnvironment = true;
			options.AllowCompressionInDevelopmentEnvironment = true;
		})
		.AddHtmlMinification(options =>
		{
			HtmlMinificationSettings settings = options.MinificationSettings;
			settings.RemoveRedundantAttributes = true;
			settings.RemoveHttpProtocolFromAttributes = true;
			settings.RemoveHttpsProtocolFromAttributes = true;
		}).AddHttpCompression(options =>
		{
			options.CompressorFactories = new List<ICompressorFactory>{
			new BuiltInBrotliCompressorFactory(new BuiltInBrotliCompressionSettings
			{
				Level = CompressionLevel.Fastest
			}),
			new DeflateCompressorFactory(new DeflateCompressionSettings
			{
				Level = CompressionLevel.Fastest
			}),
			new GZipCompressorFactory(new GZipCompressionSettings
			{
				Level = CompressionLevel.Fastest
			})
		};
		});
	}
}