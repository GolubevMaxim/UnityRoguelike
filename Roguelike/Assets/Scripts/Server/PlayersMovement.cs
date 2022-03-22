using RiptideNetworking;
using UnityEngine;

namespace Server
{
    public class PlayersMovement : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ClientToServerId.DirectionVector)]
        private static void GetSpawnPlayerMessage(ushort playerId, Message message)
        {
            MovePlayer(playerId, message.GetVector2());
            SendPosition(playerId);
        }
        
        private static void MovePlayer(ushort playerId, Vector2 direction)
        {
            Players.PlayersDictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            var previousTime = player.PreviousTime;

            var deltaTime = Time.time - previousTime;
            player.PlayerGameObject.transform.Translate(direction.normalized * deltaTime * player.Speed);

            Players.PlayersDictionary[playerId].PreviousTime = Time.time;
        }

        private static void SendPosition(ushort playerId)
        {
            Players.PlayersDictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ServerToClientId.PositionChange);
            
            message.AddUShort(playerId);
            message.AddVector3(player.PlayerGameObject.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}