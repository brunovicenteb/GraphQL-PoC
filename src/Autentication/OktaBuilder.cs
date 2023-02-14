using Okta.AspNetCore;

namespace GraphQLPoC.Autentication;

public static class OktaBuilder
{
    private const string _Bearer = "Bearer";
    public static OktaTokenSettings TokenSettings { get; private set; }

    public static void UseOka(this WebApplicationBuilder builder)
    {
        CreateConfiguration(builder);
        // services.AddCors(options =>
        // {
        //     // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
        //     options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin()
        //                                                 .AllowAnyMethod()
        //                                                 .AllowAnyHeader());
        // });
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
            options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
            options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
        })
        .AddOktaWebApi(new OktaWebApiOptions
        {
            OktaDomain =$"https://{TokenSettings.Domain}/oauth2/default",
            AuthorizationServerId = TokenSettings.AutorizationServerId,
            //Audience = audience
        });
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<OktaTokenService>();
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