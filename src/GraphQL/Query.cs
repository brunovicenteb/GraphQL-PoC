using GraphQLPoC.Data;
using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL;

public class Query
{
    [UseDbContext(typeof(AppDbContext))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Platform> GetPlatform([ScopedService] AppDbContext context)
    {
        return context.Platforms;
    }

    [UseDbContext(typeof(AppDbContext))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Command> GetCommands([ScopedService] AppDbContext context)
    {
        return context.Commands;
    }
}