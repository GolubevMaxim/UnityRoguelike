using System.Collections.Generic;

namespace Server
{
    public static class Players
    {
        public static readonly Dictionary<ushort, Player> PlayersDictionary = new();
    }
}