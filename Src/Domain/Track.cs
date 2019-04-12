using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Track : BaseEntity
    {
        public string Name { get; set; }
        public int DurationMs { get; set; }
        public bool Explicit { get; set; }
        public int DiscNumber { get; set; }
        public int TrackNumber { get; set; }
        public int Popularity { get; set; }
        public string SpotifyId { get; set; }
        public Album Album { get; set; }

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
