using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpotifyImporter.Auth;

namespace SpotifyImporter
{
    public class App
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public App(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task RunAsync()
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

            var token = new AuthResponse();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<AuthResponse>(json);
            }

            var request2 = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/users/flexx98/playlists");
            request2.Headers.Add("Authorization", $"{token.TokenType} {token.AccessToken}");

            var client2 = _clientFactory.CreateClient();
            var response2 = await client2.SendAsync(request2);

            string thing;
            if (response2.IsSuccessStatusCode)
            {
                thing = await response2.Content.ReadAsStringAsync();
            }
        }
    }
}
