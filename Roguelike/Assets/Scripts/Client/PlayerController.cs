using RiptideNetworking;
using UnityEngine;

namespace Client
{
    public class PlayerController : MonoBehaviour
    {
        private ushort Id { get; set; }

        private void Update()
        {
            if (NetworkManager.Singleton.Client == null) return;
            
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            var directionInput = new Vector2(horizontalInput, verticalInput);
            
            SendDirection(directionInput);
        }

        private static void SendDirection(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ClientToServerId.DirectionVector);

            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        [MessageHandler((ushort) NetworkManager.ServerToClientId.PositionChange)]
        private static void GetNewPositionMessage(Message message)
        {
            SetPosition(message.GetUShort(), message.GetVector3());
        }

        private static void SetPosition(ushort playerId, Vector3 position)
        {
            Debug.Log(playerId);
            foreach (var key in Players.Dictionary.Keys)
            {
                Debug.Log($"{key}: {Players.Dictionary[key]}");
            }
            Players.Dictionary[playerId].gameObject.transform.position = position;
        }

        private void OnDestroy()
        {
            Players.Dictionary.Remove(Id);
        }
    }
}