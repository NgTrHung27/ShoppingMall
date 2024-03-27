using Microsoft.EntityFrameworkCore;
using QLTTTM.models;
using DAPM.Interfaces;
using DAPM.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using DAPM.StrategyConcreteClasses;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAuthenticationStrategy, FacebookAuthenticationStrategy>();
builder.Services.AddScoped<IAuthenticationStrategy, GoogleAuthenticationStrategy>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration.GetSection("FacebookKeys").GetValue<string>("AppId");
        facebookOptions.AppSecret = builder.Configuration.GetSection("FacebookKeys").GetValue<string>("AppSecret");
        //facebookOptions.CallbackPath = "/Custome/HomeUser";
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme,googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration.GetSection("GoogleKeys").GetValue<string>("ClientId");
        googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleKeys").GetValue<string>("ClientSecret");
        //googleOptions.CallbackPath = "/Custome/HomeUser";
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
