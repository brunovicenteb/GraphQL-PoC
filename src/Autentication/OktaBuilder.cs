using Microsoft.AspNetCore.Authentication.JwtBearer;
using Okta.AspNetCore;

namespace GraphQLPoC.Autentication;

public static class OktaBuilder
{
    private const string _Bearer = "Bearer";
    public static OktaTokenSettings TokenSettings { get; private set; }

    public static void UseOka(this WebApplicationBuilder builder)
    {
        CreateConfiguration(builder);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = $"https://{TokenSettings.Domain}/oauth2/default";
            options.Audience = TokenSettings.Audience;
            options.RequireHttpsMetadata = true;
        });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<OktaTokenService>();
    }

    private static void CreateConfiguration(this WebApplicationBuilder builder)
    {
        var clientId = builder.Configuration["Authentication:Authority"];
        var clientSecret = builder.Configuration["Authentication:Secret"];
        var domain = builder.Configuration["Authentication:Domain"];
        var autorizationServerId = builder.Configuration["Authentication:ServerId"];
        var audience = builder.Configuration["Authentication:Audience"];
        TokenSettings = new OktaTokenSettings()
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Domain = domain,
            AutorizationServerId = autorizationServerId,
            Audience = audience
        };
        builder.Services.Configure<OktaTokenSettings>(c =>
        {
            c.ClientId = clientId;
            c.ClientSecret = clientSecret;
            c.Domain = domain;
            c.AutorizationServerId = autorizationServerId;
            c.Audience = audience;
        });
    }
}