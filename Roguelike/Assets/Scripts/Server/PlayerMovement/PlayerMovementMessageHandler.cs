using RiptideNetworking;
using UnityEngine;

namespace Server.PlayerMovement
{
    public class PlayerMovementMessageHandler : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ClientToServerId.Dash)]
        private static void GetDashRequest(ushort playerId, Message message)
        {
            var dashDirection = message.GetVector2();
            
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player != null)
            {
                player.GetComponent<PlayerMovement>().Dash(dashDirection);
            }
        }
        
        [MessageHandler((ushort) NetworkManager.ClientToServerId.DirectionInput)]
        private static void GetSpawnPlayerMessage(ushort playerId, Message message)
        {
            var moveDirection = message.GetVector2();
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            player.GetComponent<PlayerMovement>().Move(moveDirection);
            SendPosition(playerId);
        }

        private static void SendPosition(ushort playerId)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ServerToClientId.PlayerPositionChange);
            
            message.AddUShort(playerId);
            message.AddVector3(player.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}