using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Scripts.Player
{
    public class Movement : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        private Vector3 _localPosition;
        private NetworkTransform _networkTransform;

        [ServerRpc]
        private void ChangePositionToServerRpc(Vector3 newPos)
        {
            _networkTransform.transform.position = newPos;
        }

        private void OnEnable()
        {
            _localPosition = Vector3.zero;
            _networkTransform = GetComponent<NetworkTransform>();
        }

        private void Update()
        {
            if (!IsOwner) { return; }
            
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");

            var directionVec = new Vector3(inputX, inputY, 0);
            directionVec.Normalize();


            _localPosition += directionVec * (Time.deltaTime * _speed);
            
            if (IsClient)
            {
                ChangePositionToServerRpc(_localPosition);
            }

            if (IsServer)
            {
                _networkTransform.transform.position = _localPosition;
            }
        }
    }
}