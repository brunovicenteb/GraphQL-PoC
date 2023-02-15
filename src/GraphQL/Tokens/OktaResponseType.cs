using GraphQLPoC.Autentication;

namespace GraphQLPoC.GraphQL.Tokens;

public class OktaResponseType : ObjectType<OktaResponse>
{
    protected override void Configure(IObjectTypeDescriptor<OktaResponse> descriptor)
    {
        descriptor.Description("Return a valid Token from Okta.");
    }
}