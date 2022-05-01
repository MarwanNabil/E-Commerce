global using Ecommerce_Software_Project.Models;
global using Microsoft.AspNetCore.Mvc;
global using System.Diagnostics;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.EntityFrameworkCore;
global using Ecommerce_Software_Project.Data;
global using Ecommerce_Software_Project.ViewModels;
global using Ecommerce_Software_Project.Services;
global using System.ComponentModel.DataAnnotations.Schema;
global using Ecommerce_Software_Project.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("ConnetionDb")));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
