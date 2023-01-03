using Microsoft.Extensions.Options;
using RestSharp;

namespace Client.Services;

public interface ICauNoiApiNguon
{
	Task<string> LayTokenTheoEmailAsync(string email, string matKhau, CancellationToken cancellationToken = default);
	Task<bool> DangXuatAsync(string token, CancellationToken cancellationToken = default);
}

public class CauNoiApiNguonOptions
{
	public string UrlApi { get; set; } = string.Empty;
}

public class CauNoiApiNguon : ICauNoiApiNguon
{
	private readonly string _urlApi;

	public CauNoiApiNguon(IOptions<CauNoiApiNguonOptions> options)
	{
		_urlApi = options.Value.UrlApi;
	}

	public async Task<string> LayTokenTheoEmailAsync(string email, string matKhau, CancellationToken cancellationToken = default)
	{
		RestRequest request = new($"{_urlApi}/hethong/dangnhap", Method.Post);
		request.AddJsonBody(new
						    {
							    Email = email,
							    Password = matKhau
						    });
		RestClient client = new();
		RestResponse response = await client.ExecuteAsync(request, cancellationToken);
		if (!response.IsSuccessStatusCode)
			return string.Empty;
		return response.ToString() ?? string.Empty;
	}

	public async Task<bool> DangXuatAsync(string token, CancellationToken cancellationToken = default)
	{
		RestRequest request = new($"{_urlApi}/hethong/dangxuat", Method.Post);
		request.AddQueryParameter("token", token);
		RestClient client = new();
		RestResponse response = await client.ExecuteAsync(request, cancellationToken);
		return response.IsSuccessStatusCode;
	}
	public override string ToString()
	{
		return $"{nameof(_urlApi)}: {_urlApi}";
	}
}