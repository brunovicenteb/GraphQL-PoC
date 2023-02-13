using GraphQLPoC.Data;
using GraphQLPoC.Models;
using GraphQLPoC.GraphQL.Commands;
using GraphQLPoC.GraphQL.Platforms;

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

    [UseDbContext(typeof(AppDbContext))]
    public async Task<AddCommandPayload> AddCommandAsync(AddCommandInput input, [ScopedService] AppDbContext context)
    {
        var command = new Command
        {
            HowTo = input.HowTo,
            CommandLine = input.CommandLine,
            PlatformId = input.PlatformId
        };
        await context.Commands.AddAsync(command);
        await context.SaveChangesAsync();

        return new AddCommandPayload(command);
    }
}