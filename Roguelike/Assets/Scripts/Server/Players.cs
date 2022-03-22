using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public static class Players
    {
        public static readonly Dictionary<ushort, Player> PlayersDictionary = new();
    }
}