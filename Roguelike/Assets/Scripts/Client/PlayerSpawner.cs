using RiptideNetworking;
using Server;
using UnityEngine;

namespace Client
{
    public class PlayerSpawner : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ServerToClientId.NewPlayerSpawned)]
        private static void GetSpawnPlayerMessage(Message message)
        {
            var playerId = message.GetUShort();
            var position = message.GetVector3();

            Spawn(playerId, position);
        }
        
        
        [MessageHandler((ushort) NetworkManager.ServerToClientId.SendAllPlayersPosition)]
        private static void GetSpawnAllPlayerMessage(Message message)
        {
            foreach (var player in Players.Dictionary.Values)
                Destroy(player);
            Players.Dictionary.Clear();

            var playersCount = message.GetUShort();
            
            for (var i = 0; i < playersCount; i++)
            {
                var playerId = message.GetUShort();
                var position = message.GetVector3();

                Spawn(playerId, position);
            }
        }
        
        private static void Spawn(ushort id, Vector3 position)
        {
            var player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).gameObject;
            Players.Dictionary.Add(id, player);
        }
    }
}