using GraphQLPoC.Data;
using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL;

public class Query
{
    [UseDbContext(typeof(AppDbContext))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Platform> GetPlatform([Service(ServiceKind.Pooled)] AppDbContext context)
    {
        return context.Platforms;
    }
    
    [UseDbContext(typeof(AppDbContext))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Command> GetCommands([Service(ServiceKind.Pooled)] AppDbContext context)
    {
        return context.Commands;
    }
}