using EDC.Server.Data;
using EDC.Shared.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EDC.Server;

public static class ServiceExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, ApplicationSettings settings)
    {

        builder.Services.AddDbContext<DefaultDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddSignalR();
        builder.Services.AddCors();
        builder.Services.AddSingleton(settings);

        if (settings.Config.UseSwagger)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = settings.Auth.Issuer,
                    ValidAudience = settings.Auth.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Auth.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization();
        return builder;
    }
}