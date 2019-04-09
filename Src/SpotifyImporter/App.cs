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
        private readonly IAuthService _authService;

        public App(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory, IAuthService authService)
        {
            _clientFactory = clientFactory;
            _authService = authService;
            _appSettings = appSettings.Value;
        }

        public async Task RunAsync()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/users/flexx98/playlists");

            var authToken = await _authService.GetAuthTokenAsync();
            request2.Headers.Add("Authorization", authToken);

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
