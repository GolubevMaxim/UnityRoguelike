using System;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;

namespace Client
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private ushort port;
        [SerializeField] private string ip;

        private bool _isClientRunning;
        public RiptideNetworking.Client Client { get; set; }

        public enum ServerToClientId : ushort
        {
            NewPlayerSpawned = 1,
            PlayerPositionChange,
            SendAllPlayersPosition,
        }

        public enum ClientToServerId : ushort
        {
            SpawnRequest = 1,
            DirectionInput = 2,
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
            _isClientRunning = false;
        }

        public void StartClient()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            _isClientRunning = true;
            Client = new RiptideNetworking.Client();
            Client.Connected += OnConnection;
            
            Client.Connect($"{ip}:{port}");
        }
        
        private void OnConnection(object sender, EventArgs e)
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.SpawnRequest);
            
            Client.Send(message);
        }

        private void FixedUpdate()
        {
            if (!_isClientRunning) return;
            
            Client.Tick();
        }

        private void OnApplicationQuit()
        {
            if (!_isClientRunning) return;
            
            Client.Disconnect();
            _isClientRunning = false;
        }
    }
}