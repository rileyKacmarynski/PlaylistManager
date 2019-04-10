using System;
using System.Collections.Generic;
using System.Text;

namespace SpotifyImporter.Urls
{
    public class UrlConfig
    {
        public static string GetUserPlaylists(string username) => 
            $"https://api.spotify.com/v1/users/{username}/playlists?limit=50";

        public static string GetPlaylistTracks(string playlistId) => 
            $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";

        public static string Authorize() => "https://accounts.spotify.com/api/token";
    }
}
