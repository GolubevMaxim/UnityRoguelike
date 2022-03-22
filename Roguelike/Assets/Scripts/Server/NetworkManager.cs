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
        }

        public enum ClientToServerId : ushort
        {
            SpawnRequest = 1,
            DirectionInput,
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