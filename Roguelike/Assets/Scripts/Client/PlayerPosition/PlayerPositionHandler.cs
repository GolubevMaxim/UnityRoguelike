using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public static class PlayerPositionHandler
    {
        public static void SendDash(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.reliable, NetworkManager.ClientToServerId.Dash);

            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        public static void SendDirection(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ClientToServerId.DirectionInput);

            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        [MessageHandler((ushort) NetworkManager.ServerToClientId.PlayerPositionChange)]
        private static void GetNewPositionMessage(Message message)
        {
            var playerId = message.GetUShort();
            var position = message.GetVector3();

            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            if (playerId != NetworkManager.Singleton.Client.Id)
                player.GetComponent<PlayerUpdater>().SetPosition(position);
            else
                player.GetComponent<LocalPlayerUpdater>().SetPosition(position);
        }
    }
}