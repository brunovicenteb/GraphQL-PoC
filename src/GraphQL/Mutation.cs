using GraphQLPoC.Data;
using GraphQLPoC.Models;
using HotChocolate.Subscriptions;
using GraphQLPoC.GraphQL.Commands;
using GraphQLPoC.GraphQL.Platforms;
using Microsoft.EntityFrameworkCore;

namespace GraphQLPoC.GraphQL;

public class Mutation
{
    [UseDbContext(typeof(AppDbContext))]
    public async Task<AddPlatformPayload> AddPlatformAsync(AddPlatformInput input, [ScopedService] AppDbContext context,
        [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
    {
        var platform = new Platform
        {
            Name = input.Name
        };
        await context.Platforms.AddAsync(platform);
        await context.SaveChangesAsync();
        await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), platform, cancellationToken);
        return new AddPlatformPayload(platform);
    }

    [UseDbContext(typeof(AppDbContext))]
    public async Task<DeletePlatformPayload> DeletePlatformAsync(int platformId, [ScopedService] AppDbContext context)
    {
        var platform = await context.Platforms.FirstOrDefaultAsync(o => o.Id == platformId);
        context.Platforms.Remove(platform);
        await context.SaveChangesAsync();
        return new DeletePlatformPayload(platform);
    }

    [UseDbContext(typeof(AppDbContext))]
    public async Task<AddCommandPayload> AddCommandAsync(AddCommandInput input, [ScopedService] AppDbContext context,
        [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
    {
        var command = new Command
        {
            HowTo = input.HowTo,
            CommandLine = input.CommandLine,
            PlatformId = input.PlatformId
        };
        await context.Commands.AddAsync(command);
        await context.SaveChangesAsync();
        await eventSender.SendAsync(nameof(Subscription.OnCommandAdded), command, cancellationToken);
        return new AddCommandPayload(command);
    }
}