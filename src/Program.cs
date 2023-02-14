using GraphQLPoC.Data;
using GraphQLPoC.GraphQL;
using GraphQLPoC.Autentication;
using GraphQL.Server.Ui.Voyager;
using GraphQLPoC.GraphQL.Platforms;
using Microsoft.EntityFrameworkCore;
using GraphQLPoC.GraphQL.Commands;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Okta.AspNetCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

//builder.UseOka();

var pool = new DefaultObjectPool<AppDbContext>(new DefaultPooledObjectPolicy<AppDbContext>());

builder.Services.AddSingleton<ObjectPool<AppDbContext>>(pool);
builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
     opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandConString")));


// var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey"));
// builder.Services
//     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//             {
//                 options.TokenValidationParameters =
//                     new TokenValidationParameters
//                     {
//                         ValidIssuer = "https://auth.chillicream.com",
//                         ValidAudience = "https://graphql.chillicream.com",
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = signingKey
//                     };
//             });

// builder.Services.AddCors(options =>
// {
//     // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
//     options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin()
//                                                 .AllowAnyMethod()
//                                                 .AllowAnyHeader());
// });

var autorizationServerId = builder.Configuration["Authentication:ServerId"];
var domain = builder.Configuration["Authentication:Domain"];
var audience = builder.Configuration["Authentication:Audience"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = $"https://{domain}/oauth2/default";
    options.Audience = audience;
    options.RequireHttpsMetadata = true;
});

//IdentityModelEventSource.ShowPII = true; //Add this line
builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddType<PlatformType>()
    .AddType<CommandType>()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();


var app = builder.Build();

app.UseWebSockets()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapGraphQL();
    })
    .UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
    {
        GraphQLEndPoint = "/graphql"
    });

app.Run();