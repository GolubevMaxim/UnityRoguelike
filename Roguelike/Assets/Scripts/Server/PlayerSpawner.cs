using RiptideNetworking;
using TMPro;
using UnityEngine;

namespace Server
{
    public class PlayerSpawner : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ClientToServerId.Spawn)]
        private static void GetPlayerSpawnRequest(ushort playerId, Message message)
        {
            Debug.Log("Server get spawn cmd");
            var player = Instantiate(GameLogic.Singleton.PlayerPrefab, Vector3.zero, Quaternion.identity);
     
            Players.PlayersDictionary.Add(playerId, player);
            SendPlayerSpawn(playerId);
        }

        private static void SendPlayerSpawn(ushort playerId)
        {
            Players.PlayersDictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            var message = Message.Create(MessageSendMode.reliable, NetworkManager.ServerToClientId.PlayerSpawned);

            message.AddUShort(playerId);
            message.AddVector3(player.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}