using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Artist : BaseEntity
    {
        public string SpotifyId { get; set; }
        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}