using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;
    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public GameObject PlayerPrefabServer => playerPrefabServer;
    public GameObject PlayerPrefabClient => playerPrefabClient;
    public GameObject PlayerPrefabLocalClient => playerPrefabLocalClient;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefabServer;
    [SerializeField] private GameObject playerPrefabClient;
    [SerializeField] private GameObject playerPrefabLocalClient;

    private void Awake()
    {
        Singleton = this;
    }
}