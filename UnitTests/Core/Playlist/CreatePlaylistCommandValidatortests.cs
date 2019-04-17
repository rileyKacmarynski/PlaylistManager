using System;
using System.Collections.Generic;
using System.Text;
using Core.Playlists.CreatePlaylist;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace UnitTests.Core.Playlist
{
    public class CreatePlaylistCommandValidatorTests
    {
        private CreatePlaylistCommandValidator GetValidator()
        {
            return new CreatePlaylistCommandValidator();
        }

        [Test]
        public void CreatePlaylistCommandValidator_NameIsInvalid_Fails()
        {
            var validator = GetValidator();
            validator.ShouldHaveValidationErrorFor(c => c.Name, "");
        }
    }
}
