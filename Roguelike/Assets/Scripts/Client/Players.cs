using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Players : MonoBehaviour
    {
        public static readonly Dictionary<ushort, GameObject> Dictionary = new();
    }
}