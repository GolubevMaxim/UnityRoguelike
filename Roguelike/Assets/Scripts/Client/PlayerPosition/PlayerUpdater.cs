using UnityEngine;

namespace Client
{
    public class PlayerUpdater : MonoBehaviour
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}