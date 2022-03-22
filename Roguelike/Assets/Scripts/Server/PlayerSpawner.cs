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
     
            Players.Dictionary.Add(playerId, newPlayer);
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
                message.AddVector3(Players.Dictionary[key].PlayerGameObject.transform.position);
            }
            
            NetworkManager.Singleton.Server.Send(message, playerId);
        } 
        
        private static void SendNewPlayerSpawn(ushort playerId)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            var message = Message.Create(MessageSendMode.reliable, NetworkManager.ServerToClientId.NewPlayerSpawned);

            message.AddUShort(playerId);
            message.AddVector3(player.PlayerGameObject.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}