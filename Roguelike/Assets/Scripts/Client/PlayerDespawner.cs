using RiptideNetworking;
using UnityEngine;

namespace Client
{
    public class PlayerDespawner : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ServerToClientId.PlayerDisconnecting)]
        private static void GetSpawnPlayerMessage(Message message)
        {
            var playerId = message.GetUShort();
            
            Despawn(playerId);
        }

        private static void Despawn(ushort playerId)
        {
            Destroy(Players.Dictionary[playerId]);
            Players.Dictionary.Remove(playerId);
        }
    }
}