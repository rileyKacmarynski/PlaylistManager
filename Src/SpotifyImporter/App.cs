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
using SpotifyImporter.SpotifyResponses;

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
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/users/{_appSettings.Username}/playlists?limit=50");

            //var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/playlists/0cLVgAgfXeE9FFpUOsORqE/tracks");

            var authToken = await _authService.GetAuthTokenAsync();
            request.Headers.Add("Authorization", authToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            UserPlaylistsResponse playlistResponse;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                playlistResponse = JsonConvert.DeserializeObject<UserPlaylistsResponse>(json);
                //playlistResponse = JsonConvert.DeserializeObject<PlaylistTracksResponse>(json);

            }
        }
    }
}
