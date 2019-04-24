using Core.Playlists.CreatePlaylist;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace UnitTests.Core.Playlist
{
    [TestFixture]
    [Category(Category.Playlist)]
    public class CreatePlaylistCommandValidatorTests
    {
        private CreatePlaylistCommandValidator GetValidator()
        {
            return new CreatePlaylistCommandValidator();
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void NameValidation_IsEmpty_Fails(string value)
        {
            var validator = GetValidator();
            validator.ShouldHaveValidationErrorFor(c => c.Name, value);
        }

        [Test]
        public void NameValidation_IsTooLong_Fails()
        {
            var validator = GetValidator();
            validator.ShouldHaveValidationErrorFor(c => c.Name, "Name that is too longgggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
        }

        [Test]
        [TestCase("Bob")]
        [TestCase("A longer test name")]
        public void NameValidation_NameIsValid_Passes(string value)
        {
            var validator = GetValidator();
            validator.ShouldNotHaveValidationErrorFor(c => c.Name, value);
        }

        [Test]
        [TestCase(0)]
        [TestCase(null)]
        public void UserIdValidation_IsEmpty_Fails(int value)
        {
            var validator = GetValidator();
            validator.ShouldHaveValidationErrorFor(c => c.UserId, value);
        }

        [Test]
        public void UserIdValidation_IsEmpty_Passes()
        {
            var validator = GetValidator();
            validator.ShouldNotHaveValidationErrorFor(c => c.UserId, 1);
        }
    }
}
