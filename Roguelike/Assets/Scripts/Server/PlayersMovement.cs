using System.Collections;
using RiptideNetworking;
using UnityEngine;

namespace Server
{
    public class PlayersMovement : MonoBehaviour
    {
        [MessageHandler((ushort) NetworkManager.ClientToServerId.Dash)]
        private static void GetDashRequest(ushort playerId, Message message)
        {
            var dashDirection = message.GetVector2(); 
            GameLogic.Singleton.StartCoroutine(DashCoroutine(playerId, dashDirection));
        }
        
        [MessageHandler((ushort) NetworkManager.ClientToServerId.DirectionInput)]
        private static void GetSpawnPlayerMessage(ushort playerId, Message message)
        {
            MovePlayer(playerId, message.GetVector2());
            SendPosition(playerId);
        }
        
        private static void MovePlayer(ushort playerId, Vector2 direction)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            if (player.InDash)
            {
                Players.Dictionary[playerId].PreviousTime = Time.time;
                return;
            }

            var previousTime = player.PreviousTime;

            var deltaTime = Time.time - previousTime;
            player.PlayerGameObject.transform.Translate(direction.normalized * deltaTime * player.Speed);

            Players.Dictionary[playerId].PreviousTime = Time.time;
        }

        private static IEnumerator DashCoroutine(ushort playerId, Vector2 direction)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) yield break;
            if (player.InDash) yield break;

            var startTime = Time.time;
            
            player.InDash = true;
            
            while (Time.time - startTime < player.DashTime)
            {
                player.PlayerGameObject.transform.Translate(direction.normalized * player.Speed * 5 * Time.deltaTime);
                yield return null;
            }
            
            player.InDash = false;
        } 

        private static void SendPosition(ushort playerId)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ServerToClientId.PlayerPositionChange);
            
            message.AddUShort(playerId);
            message.AddVector3(player.PlayerGameObject.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}