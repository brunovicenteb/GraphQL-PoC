using GraphQLPoC.Data;
using GraphQLPoC.GraphQL;
using GraphQL.Server.Ui.Voyager;
using GraphQLPoC.GraphQL.Platforms;
using Microsoft.EntityFrameworkCore;
using GraphQLPoC.GraphQL.Commands;
using Microsoft.Extensions.ObjectPool;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
//     opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandConString")));
var pool = new DefaultObjectPool<AppDbContext>(new DefaultPooledObjectPolicy<AppDbContext>());
builder.Services.AddSingleton<ObjectPool<AppDbContext>>(pool);
builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
     opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandConString")));
builder.Services
    .AddGraphQLServer()
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
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});
app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
{
    GraphQLEndPoint = "/graphql"
});

app.Run();