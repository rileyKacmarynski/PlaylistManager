using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using SpotifyImporter.Comparers;
using SpotifyImporter.Services;
using SpotifyImporter.SpotifyResponses;
using SpotifyImporter.Urls;
using Track = SpotifyImporter.SpotifyResponses.Track;

namespace SpotifyImporter
{
    public class App
    {
        private readonly IApiService _apiService;
        private readonly PlaylistManagerDbContext _context;

        public App(IApiService apiService, PlaylistManagerDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        public async Task RunAsync()
        {
            var playlists = await _apiService.GetPlaylistsAsync();
            var playlistTracks = await _apiService.GetPlaylistTracksAsync(playlists.Playlists.Select(p => p.Id));
            playlistTracks = playlistTracks.Where(pt => pt.playlistId != "4lcarLEQ7hlQkhVXY8GmdK");     // for whatever reason this playlist causes problems
            var allTracks = playlistTracks.SelectMany(pt => pt.tracks.Items.Select(i => i.Track)).Where(t => t != null);

            await SeedDatabase(allTracks, playlists, playlistTracks);
        }

        // This is bad, but it's a way to get this data mostly non-duplicated into a relational structure without 
        // wasting too much time trying to get example data. 
        private async Task SeedDatabase(IEnumerable<Track> allTracks, UserPlaylistsResponse playlists, IEnumerable<(string playlistId, PlaylistTracksResponse tracks)> playlistTracks)
        {
            var artists = allTracks.Select(t => new Domain.Artist
            {
                SpotifyId = t.Artists.First().Id,
                Name = t.Artists.First().Name
            });
            await _context.Artists.AddRangeAsync(artists);
            await _context.SaveChangesAsync();

            var albums = allTracks.Select(t => new Domain.Album
            {
                SpotifyId = t.Album.Id,
                Name = t.Album.Name,
                ReleaseDate = t.Album.ReleaseDate,
                TotalTracks = t.Album.TotalTracks,
                Artist = _context.Artists.First(a => a.SpotifyId == t.Artists.FirstOrDefault().Id)
            });
            await _context.Albums.AddRangeAsync(albums);
            await _context.SaveChangesAsync();

            var domainTracks = allTracks.Select(t => new Domain.Track
            {
                SpotifyId = t.Id,
                Name = t.Name,
                DurationMs = t.DurationMs,
                Explicit = t.Explicit,
                DiscNumber = t.DiscNumber,
                TrackNumber = t.TrackNumber,
                Popularity = t.Popularity,
                Album = _context.Albums.First(a => a.SpotifyId == t.Album.Id)
            }).ToList();
            await _context.Tracks.AddRangeAsync(domainTracks);
            await _context.SaveChangesAsync();

            var domainUsers = playlists.Playlists.Select(p => new Domain.User
            {
                DisplayName = p.Owner.DisplayName,
                Email = p.Owner.DisplayName + "@domain.com",
                SpotifyId = p.Owner.Id
            }).Distinct(new SameOwner());
            await _context.Users.AddRangeAsync(domainUsers);
            await _context.SaveChangesAsync();

            var domainPlaylists = playlists.Playlists.Select(p => new Domain.Playlist
            {
                SpotifyId = p.Id,
                Name = p.Name,
                TotalTracks = p.Tracks.Total ?? 0,
                User = _context.Users.First(u => u.SpotifyId == p.Owner.Id)
            });
            await _context.Playlists.AddRangeAsync(domainPlaylists);
            await _context.SaveChangesAsync();

            var domainPlaylistTracks = playlistTracks
                .SelectMany(pt => pt.tracks.Items
                    .Select(t => new Domain.PlaylistTrack
                    {
                        Playlist = _context.Playlists.First(p => p.SpotifyId == pt.playlistId),
                        Track = _context.Tracks.First(tr => tr.SpotifyId == t.Track.Id)
                    })
                ).ToList();
            await _context.PlaylistTracks.AddRangeAsync(domainPlaylistTracks);
            await _context.SaveChangesAsync();
        }
    }
}
