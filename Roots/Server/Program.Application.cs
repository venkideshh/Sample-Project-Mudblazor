using EDC.Shared.Config;
using Serilog;

namespace EDC.Server;

public static class AppExtensions
{
    public static WebApplication ConfigureApp(this WebApplicationBuilder builder, ApplicationConfiguration settings)
    {
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (settings.UseWasmDebug)
        {
            app.UseWebAssemblyDebugging();
        }
        if (settings.UseSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        if (settings.UseErrorPage)
        {
            app.UseExceptionHandler("/Error");
        }
        if (settings.UseHsts)
        {
            app.UseHsts();
        }

        app.UseSerilogRequestLogging();

        if (settings.UseHttpsRedirection)
        {
            app.UseHttpsRedirection();
        }

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");
        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        return app;
    }
}