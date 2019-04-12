using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class User : BaseEntity
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string SpotifyId { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}