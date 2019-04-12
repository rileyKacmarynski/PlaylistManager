using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Playlist : BaseEntity
    {
        public string SpotifyId { get; set; }
        public string Name { get; set; }
        public int TotalTracks { get; set; }
        public User User { get; set; }

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}