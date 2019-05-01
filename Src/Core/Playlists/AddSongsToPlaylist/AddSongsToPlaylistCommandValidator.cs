using System.Collections.Generic;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core.Playlists.AddSongsToPlaylist
{
    public class AddSongsToPlaylistCommandValidator : AbstractValidator<AddSongsToPlaylistCommand>
    {
        public AddSongsToPlaylistCommandValidator()
        {
            RuleFor(c => c.PlaylistId).GreaterThan(0);
            RuleFor(c => c).Must(HaveSongsOrAlbums);
        }

        private static bool HaveSongsOrAlbums(AddSongsToPlaylistCommand c) =>
            c.AlbumIds.Any() || c.TrackIds.Any();
    }
}
