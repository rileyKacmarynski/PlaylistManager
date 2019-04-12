using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Album : BaseEntity
    {
        public string SpotifyId { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public int TotalTracks { get; set; }
        public Artist Artist { get; set; }
    }
}