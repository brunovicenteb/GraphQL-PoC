using System.Linq;
using GraphQLPoC.Data;
using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL;

public class Query
{
    public IQueryable<Platform> GetPlatform([Service] AppDbContext context)
    {
        return context.Platform;
    }
}