using GraphQLPoC.Data;
using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL.Commands;

public class CommandType : ObjectType<Command>
{
    protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
    {
        descriptor
            .Description("Represents any executable command");
        descriptor
            .Field(c => c.Id)
            .Description("Represents the unique ID for the command.");
        descriptor
            .Field(c => c.HowTo)
            .Description("Represents the how-to for the command.");
        descriptor
            .Field(c => c.CommandLine)
            .Description("Represents the command line.");
        descriptor
            .Field(c => c.PlatformId)
            .Description("Represents the unique ID of the platform which the command belongs.");
        descriptor
            .Field(p => p.Platform)
            .ResolveWith<Resolvers>(p => p.GetPlatform(default!, default!))
            .UseDbContext<AppDbContext>()
            .Description("This is the platform to which the command belongs");
    }

    private class Resolvers
    {
        public Platform GetPlatform([Parent] Command command, [Service(ServiceKind.Pooled)] AppDbContext context)
            => context.Platforms.FirstOrDefault(p => p.Id == command.PlatformId);
    }
}