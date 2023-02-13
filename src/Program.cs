using GraphQLPoC.Data;
using GraphQLPoC.GraphQL;
using GraphQL.Server.Ui.Voyager;
using GraphQLPoC.GraphQL.Platforms;
using Microsoft.EntityFrameworkCore;
using GraphQLPoC.GraphQL.Commands;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandConString")));
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddType<PlatformType>()
    .AddType<CommandType>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();
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