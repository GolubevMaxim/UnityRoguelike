using RiptideNetworking;
using UnityEngine;

namespace Server
{
    public class PlayerSpawner : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ClientToServerId.SpawnRequest)]
        private static void GetPlayerSpawnRequest(ushort playerId, Message message)
        {
            var player = Instantiate(GameLogic.Singleton.PlayerPrefab, Vector3.zero, Quaternion.identity);
            var newPlayer = new Player(player, 100);
     
            Players.PlayersDictionary.Add(playerId, newPlayer);
            SendNewPlayerSpawn(playerId);
        }

        private static void SendAllPlayers(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.reliable,
                NetworkManager.ServerToClientId.SendAllPlayersPosition);

        } 
        
        private static void SendNewPlayerSpawn(ushort playerId)
        {
            Players.PlayersDictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            var message = Message.Create(MessageSendMode.reliable, NetworkManager.ServerToClientId.NewPlayerSpawned);

            message.AddUShort(playerId);
            message.AddVector3(player.PlayerGameObject.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}