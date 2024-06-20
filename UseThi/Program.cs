using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Data.Data;
using Data.Data.Entities;
using UseThi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShopDbContextConnection") ?? throw new InvalidOperationException("Не знайдено рядок підключення 'ShopDbContextConnection'.");

// Налаштування сервісів
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ShopDbContext>();

// Додавання CryptoService як singleton
var apiKey = "484798a5-f1d0-4f4e-b9b4-131776be9383";
builder.Services.AddSingleton(new CryptoService(apiKey));

// Додавання AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Додавання підтримки сесій
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Налаштування тайм-ауту сесії
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.RootDirectory = "/Views";
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    // Конвенції сторінок не потрібно налаштовувати для контролерів
});

var app = builder.Build();

// Налаштування middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
