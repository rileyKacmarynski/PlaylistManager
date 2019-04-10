using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpotifyImporter.Services;
using SpotifyImporter.SpotifyResponses;
using SpotifyImporter.Urls;

namespace SpotifyImporter
{
    public class App
    {
        private readonly IApiService _apiService;

        public App(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task RunAsync()
        {
            var playlists = await _apiService.GetPlaylistsAsync();

            var tracks = await _apiService.GetPlaylistTracksAsync(playlists.Playlists.Select(p => p.Id));
        }

    }
}
