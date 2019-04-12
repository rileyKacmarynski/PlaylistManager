namespace Domain
{
    public class PlaylistTrack : BaseEntity
    {
        public Playlist Playlist { get; set; }

        public Track Track { get; set; }
    }
}