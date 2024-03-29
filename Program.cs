using Microsoft.EntityFrameworkCore;
using QLTTTM.models;
using Microsoft.AspNetCore.Http;
using DAPM.Interfaces;
using DAPM.Repository;
using DAPM.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Admin/Login";
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["FacebookKeys:AppId"];
    options.AppSecret = builder.Configuration["FacebookKeys:AppSecret"];
});

builder.Services.AddDbContext<DataSQLContext>(options =>
{
options.UseSqlServer(builder.Configuration.GetConnectionString("DATABASESQL"), 
    options => options.EnableRetryOnFailure());
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Đặt thời gian session là 30 phút
});

builder.Services.AddDistributedMemoryCache(); // Thêm dịch vụ bộ nhớ cache


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


app.UseAuthentication();
app.UseAuthorization();


app.UseSession();

//app.UseMiddleware<SessionTimeoutMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.Run();