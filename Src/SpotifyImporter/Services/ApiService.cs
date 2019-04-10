using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpotifyImporter.SpotifyResponses;
using SpotifyImporter.Urls;

namespace SpotifyImporter.Services
{
    public class ApiService : IApiService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly Lazy<Task<string>> _authToken;

        public ApiService(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;

            _authToken = new Lazy<Task<string>>(async () => await GetAuthTokenAsync());
        }
        
        public async Task<UserPlaylistsResponse> GetPlaylistsAsync() => 
            await GetApiDataAsync<UserPlaylistsResponse>(UrlConfig.GetUserPlaylists(_appSettings.Username));

        public async Task<IEnumerable<(string playlistId, PlaylistTracksResponse tracks)>> GetPlaylistTracksAsync(IEnumerable<string> playlistIds)
        {
            var playlistTracksTasks = playlistIds.Select(GetTracksAsync);
            return await Task.WhenAll(playlistTracksTasks);
        }

        private async Task<(string playlistId, PlaylistTracksResponse tracks)> GetTracksAsync(string id)
        {
            var tracks = await GetApiDataAsync<PlaylistTracksResponse>(UrlConfig.GetUserPlaylists(_appSettings.Username));
            return (id, tracks);
        }

        private async Task<T> GetApiDataAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("Authorization", await _authToken.Value);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to fulfill request.");

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, UrlConfig.Authorize());

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

    public interface IApiService
    {
        Task<UserPlaylistsResponse> GetPlaylistsAsync();
        Task<IEnumerable<(string playlistId, PlaylistTracksResponse tracks)>> GetPlaylistTracksAsync(IEnumerable<string> playlistIds);
    }
}
