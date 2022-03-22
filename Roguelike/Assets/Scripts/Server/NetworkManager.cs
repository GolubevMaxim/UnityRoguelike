using System;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

namespace Server
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        private bool _isServerRunning;
        public RiptideNetworking.Server Server { get; set; }

        public enum ServerToClientId : ushort
        {
            NewPlayerSpawned = 1,
            PlayerPositionChange,
            SendAllPlayersPosition,
            PlayerDisconnecting,
        }

        public enum ClientToServerId : ushort
        {
            SpawnRequest = 1,
            DirectionInput,
            ClientDisconnecting,
        }
        
        private static NetworkManager _singleton;
        public static NetworkManager Singleton
        {
            get => _singleton;
            set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
            _isServerRunning = false;
        }

        public void StartServer()
        {
            Application.targetFrameRate = 60;

            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            _isServerRunning = true;
            Server = new RiptideNetworking.Server();
            Server.Start(port, maxClientCount);
            Server.ClientDisconnected += OnClientDisconnect;
        }
        
        private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e)
        {
            var playerId = e.Id;
            
            RemovePlayer(playerId);
            SendPlayerDespawn(playerId);
        }
        
        private static void RemovePlayer(ushort playerId)
        {
            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;
            
            Players.Dictionary.Remove(playerId);
            Destroy(player.PlayerGameObject);
        }
        
        private static void SendPlayerDespawn(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.PlayerDisconnecting);

            message.AddUShort(playerId);
            Singleton.Server.SendToAll(message);
        }

        private void FixedUpdate()
        {
            if (!_isServerRunning) return;
            
            Server.Tick();
        }

        private void OnApplicationQuit()
        {
            if (!_isServerRunning) return;
            
            Server.Stop();
            _isServerRunning = false;
        }
    }
}