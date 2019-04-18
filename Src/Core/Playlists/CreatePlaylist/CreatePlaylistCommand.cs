using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using MediatR;

namespace Core.Playlists.CreatePlaylist
{
    public class CreatePlaylistCommand : IRequest
    {
        public string Name { get; set; }
        public int UserId { get; set; }

        public class Handler : IRequestHandler<CreatePlaylistCommand, Unit>
        {
            private readonly IPlaylistManagerDbContext _context;

            public Handler(IPlaylistManagerDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.UserId);
                if(user == null)
                {
                    throw
                }
            }
        }
    }
}
