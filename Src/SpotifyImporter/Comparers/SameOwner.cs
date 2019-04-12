using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace SpotifyImporter.Comparers
{

    public class SameOwner : EqualityComparer<Domain.User>
    {
        public override bool Equals(User x, User y)
        {
            return x.DisplayName == y.DisplayName;
        }

        public override int GetHashCode(User obj)
        {
            return obj.DisplayName.GetHashCode();
        }
    }
}
