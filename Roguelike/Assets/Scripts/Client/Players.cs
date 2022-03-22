using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Players : MonoBehaviour
    {
        public static Dictionary<ushort, GameObject> Dictionary = new Dictionary<ushort, GameObject>();
    }
}