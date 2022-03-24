using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public static class Players 
    {
        public static readonly Dictionary<ushort, GameObject> Dictionary = new();

        public static void Spawn(ushort playerId, Vector3 position)
        {
            var player = Object.Instantiate(GameLogic.Singleton.PlayerPrefabServer, position, Quaternion.identity);
            
            Dictionary.Add(playerId, player);
        }
    }
}