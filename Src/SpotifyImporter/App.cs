using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpotifyImporter.Comparers;
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
            var playlistTracks = await _apiService.GetPlaylistTracksAsync(playlists.Playlists.Select(p => p.Id));
            playlistTracks = playlistTracks.Where(pt => pt.playlistId != "4lcarLEQ7hlQkhVXY8GmdK");     // for whatever reason this playlist causes problems
            var allTracks = playlistTracks.SelectMany(pt => pt.tracks.Items.Select(i => i.Track)).Where(t => t != null);

            var domainTracks = allTracks.Select(t => new Domain.Track
            {
                SpotifyId = t.Id,
                Name = t.Name,
                DurationMs = t.DurationMs,
                Explicit = t.Explicit,
                DiscNumber = t.DiscNumber,
                TrackNumber = t.TrackNumber,
                Popularity = t.Popularity,
                Album = new Domain.Album
                {
                    SpotifyId = t.Album.Id,
                    Name = t.Album.Name,
                    ReleaseDate = t.Album.ReleaseDate,
                    TotalTracks = t.Album.TotalTracks,
                    Artist = t.Artists.Select(a => new Domain.Artist
                    {
                        SpotifyId = a.Id,
                        Name = a.Name
                    }).First(),
                }
            }).ToList();

            var domainUsers = playlists.Playlists.Select(p => new Domain.User
            {
                DisplayName = p.Owner.DisplayName,
                Email = p.Owner.DisplayName + "@domain.com",
                SpotifyId = p.Owner.Id
            }).Distinct(new SameOwner());

            var domainPlaylists = playlists.Playlists.Select(p => new Domain.Playlist
            {
                SpotifyId = p.Id,
                Name = p.Name,
                TotalTracks = p.Tracks.Total ?? 0,
                User = domainUsers.First(u => p.Owner.Id == u.SpotifyId)
            });

            var domainPlaylistTracks = playlistTracks
                .SelectMany(pt => pt.tracks.Items
                    .Select(t => new Domain.PlaylistTrack
                    {
                        Playlist = domainPlaylists.First(p => p.SpotifyId == pt.playlistId),
                        Track = domainTracks.First(tr => tr.SpotifyId == t.Track.Id)
                    })
                ).ToList();


        }

    }
}
