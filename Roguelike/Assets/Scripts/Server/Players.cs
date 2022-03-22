using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public static class Players
    {
        public static Dictionary<ushort, GameObject> PlayersDictionary = new Dictionary<ushort, GameObject>();
    }
}