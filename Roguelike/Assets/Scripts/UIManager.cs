using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void StartClient()
    {
        Client.NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        Server.NetworkManager.Singleton.StartServer();
    }
}