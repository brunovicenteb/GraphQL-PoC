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

builder.UseOka();

builder.Services.AddSingleton<ObjectPool<AppDbContext>>(pool);
builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
     opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandConString")));

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