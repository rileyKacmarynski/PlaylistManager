using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Common;
using API.Controllers;
using API.Dtos;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace UnitTests.Core.Playlist
{
    [Category(Category.Playlist)]
    public class PlaylistControllerTests
    {
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
    }
}
