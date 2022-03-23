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

            Move();
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
                Dash();
        }
        private Vector2 GetDirectionFromKeyBoard()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            var directionInput = new Vector2(horizontalInput, verticalInput);
            
            return directionInput;
        }
        
        private void Move()
        {
            var movementDirection = GetDirectionFromKeyBoard();
            SendDirection(movementDirection);
        }
        
        private void Dash()
        {
            var dashDirection = GetDirectionFromKeyBoard();
            SendDash(dashDirection);
        }

        private static void SendDash(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.reliable, NetworkManager.ClientToServerId.Dash);

            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        private static void SendDirection(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.unreliable, NetworkManager.ClientToServerId.DirectionInput);

            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        [MessageHandler((ushort) NetworkManager.ServerToClientId.PlayerPositionChange)]
        private static void GetNewPositionMessage(Message message)
        {
            SetPosition(message.GetUShort(), message.GetVector3());
        }

        private static void SetPosition(ushort playerId, Vector3 position)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            player.gameObject.transform.position = position;
        }

        private void OnDestroy()
        {
            Players.Dictionary.Remove(Id);
        }
    }
}