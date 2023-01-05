#region

using Client.Areas.Admin.Contexts;
using Client.Middlewares;
using Client.Services;
using Client.ThietLap;
using Microsoft.EntityFrameworkCore;
using RezUtility.BackgroundServices;
using RezUtility.Services;
using RezUtility.Tasks;
using System.Text;
using WebMarkupMin.AspNetCore7;
using ITokenDangXuatService=Client.Services.ITokenDangXuatService;
using TokenDangXuatService=Client.Services.TokenDangXuatService;

#endregion

Console.OutputEncoding = Encoding.UTF8;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClientDbContext>(options => options.UseSqlite("DataSource=client.database"));
builder.Services.ThietLapMvcjson();
builder.ThietLapXacThuc();
builder.Services.ThietLapSession();
builder.Services.WebMarkupMin();
builder.ThietLapApiMayChu();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton<ITokenDangXuatService, TokenDangXuatService>();
builder.Services.AddHostedService<XoaTokenBackgroundService>();
builder.Services.AddTransient<ICauNoiApiNguon, CauNoiApiNguon>();
builder.Services.Configure<CauNoiApiNguonOptions>(options =>
												  {
													  options.UrlApi = builder.Configuration["MayChuApi:DiaChi"] ?? throw new Exception();
												  });

WebApplication app = builder.Build();
CheckTask.Check(app.Services);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseKiemTraCookie();
app.UseXacThuc();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
if (!app.Environment.IsDevelopment())
	app.UseWebMarkupMin();
app.MapControllerRoute("default", "{area=admin}/{controller=nguoidung}/{action=index}");

app.Run();