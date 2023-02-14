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

var builder = WebApplication.CreateBuilder(args);

//builder.UseOka();

var pool = new DefaultObjectPool<AppDbContext>(new DefaultPooledObjectPolicy<AppDbContext>());

builder.Services.AddSingleton<ObjectPool<AppDbContext>>(pool);
builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
     opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandConString")));


var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey"));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidIssuer = "https://auth.chillicream.com",
                        ValidAudience = "https://graphql.chillicream.com",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey
                    };
            });

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
app.UseWebSockets();
var routing = app.UseRouting();
//  .UseEndpoints(endpoints =>
//  {
//      endpoints.MapGraphQL();
//  });
//app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
routing.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});
app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
{
    GraphQLEndPoint = "/graphql"
});
app.Run();