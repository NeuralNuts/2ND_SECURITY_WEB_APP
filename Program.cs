using _2ND_SECURITY_WEB_APP.Context;
using _2ND_SECURITY_WEB_APP.Repository;
using _2ND_SECURITY_WEB_APP.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<FileUploaderServices>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(500);
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Au",
//          authBuilder =>
//          {
//              authBuilder.RequireRole("guest");
//          });
//});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    //Sets the defualt locatoin to redirect the user when login is required
    options.LoginPath = "/Home/Login";
    //Sets the path to direct the user if they try to access a resource that
    //their role does not have access to.
    options.AccessDeniedPath = "/Home/Error";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    //Refreshes the timeapsn back to max wheneven the cookie is used when under
    //half its lifetime.
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
