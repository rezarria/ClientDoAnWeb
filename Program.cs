#region

using Client.Areas.Admin.Contexts;
using Client.ThietLap;
using Microsoft.EntityFrameworkCore;
using WebMarkupMin.AspNetCore7;

#endregion

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClientDbContext>(options => options.UseSqlite("DataSource=client.database"));
builder.Services.ThietLapMvcjson();
builder.Services.WebMarkupMin();
builder.ThietLapApiMayChu();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()){
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseResponseCaching();
if (!app.Environment.IsDevelopment())
	app.UseWebMarkupMin();
app.MapControllerRoute("default", "{area=admin}/{controller=nguoidung}/{action=index}");

app.Run();
