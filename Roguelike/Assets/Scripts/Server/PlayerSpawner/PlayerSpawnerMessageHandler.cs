using RiptideNetworking;
using UnityEngine;

namespace Server.PlayerSpawner
{
    public class PlayerSpawnerMessageHandler : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ClientToServerId.SpawnRequest)]
        private static void GetPlayerSpawnRequest(ushort playerId, Message message)
        {
            Players.Spawn(playerId, Vector3.zero);
            SendNewPlayerSpawn(playerId);
            SendAllPlayers(playerId);
        }

        private static void SendAllPlayers(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.reliable,
                NetworkManager.ServerToClientId.SendAllPlayersPosition);

            var playerCount = (ushort) Players.Dictionary.Count;
            message.AddUShort(playerCount);

            foreach (var key in Players.Dictionary.Keys)
            {
                message.AddUShort(key);
                message.AddVector3(Players.Dictionary[key].transform.position);
            }
            
            NetworkManager.Singleton.Server.Send(message, playerId);
        } 
        
        private static void SendNewPlayerSpawn(ushort playerId)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            var message = Message.Create(MessageSendMode.reliable, NetworkManager.ServerToClientId.NewPlayerSpawned);

            message.AddUShort(playerId);
            message.AddVector3(player.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}