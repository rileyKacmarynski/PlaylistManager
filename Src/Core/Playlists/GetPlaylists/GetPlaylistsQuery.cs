using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Playlists.GetPlaylists
{
    public class GetPlaylistsQuery : IRequest<IEnumerable<Playlist>>
    {
        public int? UserId { get; set; }
        public string Name { get; set; }

        public class Handler : IRequestHandler<GetPlaylistsQuery, IEnumerable<Playlist>>
        {
            private readonly IPlaylistManagerDbContext _context;

            public Handler(IPlaylistManagerDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Playlist>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
            {
                var query = _context.Playlists.Include(p => p.User).AsQueryable();

                if (request.UserId != null && request.UserId.Value != 0)
                {
                    query = query.Where(p => p.User.Id == request.UserId);
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.Name.Contains(request.Name));
                }

                return await query.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
