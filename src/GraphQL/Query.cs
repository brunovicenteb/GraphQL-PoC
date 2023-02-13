using GraphQLPoC.Data;
using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL;

public class Query
{
    [UseDbContext(typeof(AppDbContext))]
    [UseProjection]
    public IQueryable<Platform> GetPlatform([ScopedService] AppDbContext context)
    {
        return context.Platforms;
    }

    [UseDbContext(typeof(AppDbContext))]
    [UseProjection]
    public IQueryable<Command> GetCommands([ScopedService] AppDbContext context)
    {
        return context.Commands;
    }
}