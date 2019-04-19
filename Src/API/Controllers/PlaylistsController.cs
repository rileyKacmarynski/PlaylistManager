using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Common;
using API.Dtos;
using Core.Playlists.CreatePlaylist;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlaylistsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto, CancellationToken token)
        {
            var command = new CreatePlaylistCommand {Name = dto.Name, UserId = dto.UserId};
            await _mediator.Send(command, token);

            return new JsonResult(Result.Ok());
        }
    }
}