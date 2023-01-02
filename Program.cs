#region

using Client.Areas.Admin.Contexts;
using Client.BackgroundServices;
using Client.Middlewares;
using Client.Services;
using Client.ThietLap;
using Microsoft.EntityFrameworkCore;
using WebMarkupMin.AspNetCore7;

#endregion

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

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseXacThuc();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseResponseCaching();
if (!app.Environment.IsDevelopment())
	app.UseWebMarkupMin();
app.MapControllerRoute("default", "{area=admin}/{controller=nguoidung}/{action=index}");

app.Run();