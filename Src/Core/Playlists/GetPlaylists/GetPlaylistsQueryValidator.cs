using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Core.Playlists.GetPlaylists
{
    public class GetPlaylistsQueryValidator : AbstractValidator<GetPlaylistsQuery>
    {
        public GetPlaylistsQueryValidator()
        {
            RuleFor(q => q.Name).MaximumLength(200);
            RuleFor(q => q).Must(SupplyAtLeastOneFilter);
        }

        private static bool SupplyAtLeastOneFilter(GetPlaylistsQuery q) => 
            !(string.IsNullOrWhiteSpace(q.Name) && MissingUserId(q));

        private static bool MissingUserId(GetPlaylistsQuery q) => (q.UserId == null || q.UserId.Value == 0);
    }
}
