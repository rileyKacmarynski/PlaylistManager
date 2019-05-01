using System.Threading;
using System.Threading.Tasks;
using API.Common;
using API.Controllers;
using API.Dtos;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace UnitTests.Core.Playlists
{
    [Category(Category.Playlist)]
    public class PlaylistControllerTests
    {
        // I'm only testing success cases here. 
        // If the core layer throws an error it will be handled by the CustomExceptionFilter class
        // We can test that class as an integration test
        [Test]
        public async Task CreatePlaylist_Succeeds_ReturnsSuccessResult()
        {
            var dto = new CreatePlaylistDto {Name = "Test", UserId = 1};
            var fakeMediator = A.Fake<IMediator>();

            var controller = new PlaylistsController(fakeMediator);
            var json = (JsonResult)await controller.CreatePlaylist(dto, CancellationToken.None);
            var res = (Result) json.Value;

            Assert.IsTrue(res.Success);
        }

        [Test]
        public async Task GetPlaylist_Succeeds_ReturnsSuccessResult()
        {
            var fakeMediator = A.Fake<IMediator>();

            var controller = new PlaylistsController(fakeMediator);
            var json = (JsonResult) await controller.GetPlaylists(1, string.Empty, CancellationToken.None);
            var res = (Result) json.Value;

            Assert.IsTrue(res.Success);
        }
    }
}
