using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Common;
using API.Dtos;
using Core.Playlists.CreatePlaylist;
using Core.Playlists.GetPlaylists;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpGet]
        public async Task<IActionResult> GetPlaylists([FromQuery] int? userId, [FromQuery] string name,
            CancellationToken token)
        {
            var query = new GetPlaylistsQuery {UserId = userId, Name = name};
            var playlists = await _mediator.Send(query, token);

            return new JsonResult(Result.Ok(playlists));
        }
    }
}