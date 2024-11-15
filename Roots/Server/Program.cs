using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.ResponseCompression;
using EDC.Server;
using EDC.Shared.Config;
using Serilog;
using System.Net;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddMudServices();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

var settings = builder.Configuration.GetSection("App").Get<ApplicationSettings>();
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

string applicationUrl = configuration.GetSection("ApplicationSettings")["ApplicationUrl"];
// Configure Services
builder.ConfigureServices(settings);

// Configure App
builder.ConfigureApp(settings.Config).Run();

// Add services to the container.

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

var app = builder.Build();
app.UseStaticFiles();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();


//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

//app.Run();
