using System;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;

namespace Client
{
    public class NetworkManager : MonoBehaviour
    {
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

        public RiptideNetworking.Client Client { get; set; }
        
        private bool _isClientRunning;

        [SerializeField] private ushort port;
        [SerializeField] private string ip;
        
        public enum ServerToClientId : ushort
        {
            PlayerSpawned = 1,
            PositionChange = 2,
        }

        public enum ClientToServerId : ushort
        {
            Spawn = 1,
            DirectionVector = 2,
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
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.Spawn);
            
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