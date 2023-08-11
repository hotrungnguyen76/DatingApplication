using System.Text;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration config)
        {
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => 
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding
                            .UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return Services;
        }
    }
}