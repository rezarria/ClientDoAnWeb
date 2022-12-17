#region

using Client.Contexts;
using Client.ThietLap;
using Microsoft.EntityFrameworkCore;
using WebMarkupMin.AspNetCore7;

#endregion

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClientDbContext>(options => options.UseSqlite("DataSource=client.database"));
builder.Services.ThietLapMVCJSON();
builder.Services.WebMarkupMin();
builder.ThietLapApiMayChu();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseResponseCaching();
app.UseWebMarkupMin();
app.MapControllerRoute(
	"default",
	"{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	"areas",
	"{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


app.Run();