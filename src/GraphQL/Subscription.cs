using GraphQLPoC.Models;

namespace GraphQLPoC.GraphQL;

public class Subscription
{
    [Subscribe]
    [Topic]
    public Platform OnPlatformAdded([EventMessage] Platform platform)
        => platform;
}