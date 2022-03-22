using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

namespace Server
{
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkManager _singleton;
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

        public RiptideNetworking.Server Server { get; set; }

        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        private bool _isServerRunning;

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