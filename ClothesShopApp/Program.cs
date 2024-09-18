using ClothesShopApp.Areas.Account.Models;
using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/Account/Login/Index";
    options.LogoutPath = "/Account/Logout/Index";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("https://localhost:7255")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<EmailSender, EmailSender>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductImageSerivce>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<VariantService>();
builder.Services.AddScoped<VariantValueService>();
builder.Services.AddScoped<ProductVariantService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<SmsService>();
builder.Services.AddScoped<ReviewService>();



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
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


app.MapControllerRoute(
    name: "default",
    pattern: "{area=WebPage}/{controller=HomePage}/{action=Index}/{id?}");
    

app.Run();
