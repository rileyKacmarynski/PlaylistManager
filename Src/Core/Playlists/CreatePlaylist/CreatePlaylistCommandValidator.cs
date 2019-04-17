using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Core.Playlists.CreatePlaylist
{
    public class CreatePlaylistCommandValidator : AbstractValidator<CreatePlaylistCommand>
    {
        public CreatePlaylistCommandValidator()
        {
            RuleFor(c => c.Name).Must(n => !string.IsNullOrWhiteSpace(n));
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}
