using System;
using System.Collections.Generic;
using System.Text;
using Core.Playlists;
using Core.Playlists.GetPlaylists;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTests.Core.Playlist
{
    [Category(Category.Playlist)]
    public class GetPlaylistsQueryValidatorTests
    {
        private GetPlaylistsQueryValidator GetValidator()
        {
            return new GetPlaylistsQueryValidator();;
        }

        [Test]
        public void Name_ValidLength_PassesValidation()
        {
            var validator = GetValidator();
            validator.ShouldNotHaveValidationErrorFor(q => q.Name, "Valid Name");
        }

        [Test]
        public void Name_InvalidLength_FailsValidation()
        {
            var validator = GetValidator();
            validator.ShouldHaveValidationErrorFor(q => q.Name, "Tooo Loooo000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooog");
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase(" ", 0)]
        [TestCase(null, 0)]
        public void GetPlaylistsQueryValidator_NoUserIdOrName_FailsValidation(string name, int? userId)
        {
            var validator = GetValidator();
            var query = new GetPlaylistsQuery {Name = name, UserId = userId};

            var res = validator.Validate(query);

            Assert.False(res.IsValid);
        }

        [Test]
        [TestCase("bob", null)]
        [TestCase("", 1)]
        [TestCase("bob", 0)]
        [TestCase(" ", 1)]
        [TestCase(null, 1)]
        [TestCase("bob", 1)]
        public void GetPlaylistsQueryValidator_FilterSupplied_PassesValidation(string name, int? userId)
        {
            var validator = GetValidator();
            var query = new GetPlaylistsQuery { Name = name, UserId = userId };

            var res = validator.Validate(query);

            Assert.True(res.IsValid);
        }
    }
}
