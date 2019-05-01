using System.Collections.Generic;
using Core.Playlists.AddSongsToPlaylist;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace UnitTests.Core.Playlists
{
    [Category(Category.Playlist)]
    public class AddSongsToPlaylistCommandValidatorTests
    {
        private AddSongsToPlaylistCommandValidator GetValidator()
        {
            return new AddSongsToPlaylistCommandValidator();
        }

        [Test]
        public void PlaylistId_Exists_Passes()
        {
            var validator = GetValidator();
            validator.ShouldNotHaveValidationErrorFor(c => c.PlaylistId, 1);
        }

        [Test]
        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-1)]
        public void PlaylistId_DoesNotExist_Fails(int value)
        {
            var validator = GetValidator();
            validator.ShouldHaveValidationErrorFor(c => c.PlaylistId, value);
        }

        [Test]
        public void MustHaveSongsOrAlbums_HasNeither_Fails()
        {
            var validator = GetValidator();
            var command = new AddSongsToPlaylistCommand
            {
                PlaylistId = 1
            };

            var res = validator.Validate(command);

            Assert.IsFalse(res.IsValid);
        }

        [Test]
        public void MustHaveSongsOrAlbums_HasEmptyLists_Fails()
        {
            var validator = GetValidator();
            var command = new AddSongsToPlaylistCommand
            {
                PlaylistId = 1,
                AlbumIds = new List<int>(),
                TrackIds = new List<int>()
            };

            var res = validator.Validate(command);

            Assert.IsFalse(res.IsValid);
        }

        [Test]
        public void MustHaveSongsOrAlbums_HasBoth_Passes()
        {
            var validator = GetValidator();
            var command = new AddSongsToPlaylistCommand
            {
                PlaylistId = 1,
                AlbumIds = new List<int> { 1 },
                TrackIds = new List<int> { 1, 2}
            };

            var res = validator.Validate(command);

            Assert.IsTrue(res.IsValid);
        }

        [Test]
        public void MustHaveSongsOrAlbums_HasSongs_Passes()
        {
            var validator = GetValidator();
            var command = new AddSongsToPlaylistCommand
            {
                PlaylistId = 1,
                TrackIds = new List<int> { 1, 2 }
            };

            var res = validator.Validate(command);

            Assert.IsTrue(res.IsValid);
        }

        [Test]
        public void MustHaveSongsOrAlbums_HasAlbums_Passes()
        {
            var validator = GetValidator();
            var command = new AddSongsToPlaylistCommand
            {
                PlaylistId = 1,
                AlbumIds = new List<int> { 1, 2 }
            };

            var res = validator.Validate(command);

            Assert.IsTrue(res.IsValid);
        }
    }
}
