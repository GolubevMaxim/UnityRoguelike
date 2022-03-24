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
        public RiptideNetworking.Client Client { get; set; }

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
            Dash,
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
            
            Client = new RiptideNetworking.Client();
        }

        public void StartClient()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

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
            if (Client.IsNotConnected) return;
            
            Client.Tick();
        }

        private void OnApplicationQuit()
        {
            if (Client.IsConnected) return;
            
            Client.Disconnect();
        }
    }
}