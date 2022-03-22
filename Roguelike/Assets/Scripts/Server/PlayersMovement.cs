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
            Players.PlayersDictionary[playerId].transform.Translate(direction.normalized );
        }

        private static void SendPosition(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ServerToClientId.PositionChange);
            
            message.AddUShort(playerId);
            message.AddVector3(Players.PlayersDictionary[playerId].transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}