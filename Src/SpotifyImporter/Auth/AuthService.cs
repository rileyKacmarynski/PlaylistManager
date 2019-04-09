using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace SpotifyImporter.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public AuthService(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            var content = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
            request.Content = new FormUrlEncodedContent(content);

            var authHeader = Base64UrlEncoder.Encode($"{_appSettings.ClientId}:{_appSettings.ClientSecret}");
            request.Headers.Add("Authorization", $"Basic {authHeader}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to Authenticate with Spotify servers.");

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AuthResponse>(json);
            return $"{token.TokenType} {token.AccessToken}";
        }
    }

    public interface IAuthService
    {
        Task<string> GetAuthTokenAsync();
    }
}
