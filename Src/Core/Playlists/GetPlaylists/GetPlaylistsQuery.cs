using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;

namespace Core.Playlists.GetPlaylists
{
    public class GetPlaylistsQuery : IRequest<IEnumerable<Playlist>>
    {
        public int? UserId { get; set; }
        public string Name { get; set; }

        public class Handler : IRequestHandler<GetPlaylistsQuery, IEnumerable<Playlist>>
        {
            public Task<IEnumerable<Playlist>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
