using GraphQLPoC.Data;
using GraphQLPoC.Models;
using GraphQLPoC.Autentication;

namespace GraphQLPoC.GraphQL;

public class Query
{
    public async Task<OktaResponse> GetToken([Service] OktaTokenService tokenService)
    {
        return await tokenService.GetToken();
    }

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