using GraphQLPoC.Data;
using GraphQLPoC.GraphQL.Platforms;
using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL;

public class Mutation
{
    [UseDbContext(typeof(AppDbContext))]
    public async Task<AddPlatformPayload> AddPlatformAsync(AddPlatformInput input, [ScopedService] AppDbContext context)
    {
        var platform = new Platform
        {
            Name = input.Name
        };
        await context.Platforms.AddAsync(platform);
        await context.SaveChangesAsync();
        
        return new AddPlatformPayload(platform);
    }
}