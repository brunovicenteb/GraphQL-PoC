using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GraphQLPoC.Autentication;

public class OktaTokenService
{
    private OktaResponse _Token = new();

    public async Task<OktaResponse> GetToken()
    {
       if (!_Token.IsValidAndNotExpiring)
           _Token = await GetNewAccessToken();
       return _Token;
    }

    private async Task<OktaResponse> GetNewAccessToken()
    {
       using var client = new HttpClient();
       var clientId = OktaBuilder.TokenSettings.ClientId;
       var clientSecret = OktaBuilder.TokenSettings.ClientSecret;
       var clientCreds = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
       client.DefaultRequestHeaders.Authorization =
           new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

       var postMessage = new Dictionary<string, string>
       {
           { "grant_type", "client_credentials" },
           { "scope", "access_token" }
       };
       var tokenUrl = $"https://{OktaBuilder.TokenSettings.Domain}/oauth2/default/v1/token";
       var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
       {
           Content = new FormUrlEncodedContent(postMessage)
       };

       var response = await client.SendAsync(request);
       if (response.IsSuccessStatusCode)
       {
           //var json = await response.Content.ReadAsStringAsync();
           //token = JsonConvert.DeserializeObject<OktaToken>(json);
           //token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
           var jsonSerializerSettings = new JsonSerializerSettings();
           var json = await response.Content.ReadAsStringAsync();
           var token = JsonConvert.DeserializeObject<OktaResponse>(json, jsonSerializerSettings);
           token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
           return token;
       }
       throw new ApplicationException("Unable to retrieve access token from Okta");
    }

    public async Task<OktaResponse> GetToken(string username, string password)
    {
        using (var client = new HttpClient())
        {
            var clientCreds = Encoding.UTF8.GetBytes($"{OktaBuilder.TokenSettings.ClientId}:{OktaBuilder.TokenSettings.ClientSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "password");
            postMessage.Add("username", username);
            postMessage.Add("password", password);
            postMessage.Add("scope", "openid");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{OktaBuilder.TokenSettings.Domain}/oauth2/default/v1/token")
            {
                Content = new FormUrlEncodedContent(postMessage)
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                var json = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<OktaResponse>(json, jsonSerializerSettings);
                token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                return token;
            }
            var error = await response.Content?.ReadAsStringAsync();
            throw new ApplicationException(error);
        }
    }
}