using System.Collections;
using UnityEngine;

namespace Server.PlayerMovement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float dashTime;
        
        private bool _inDash;
        private float _previousTime;
        public void Move(Vector2 direction)
        {
            if (_inDash)
            {
                _previousTime = Time.time;
                return;
            }

            var previousTime = _previousTime;

            var deltaTime = Time.time - previousTime;
            transform.Translate(direction.normalized * deltaTime * speed);

            _previousTime = Time.time;
        }

        public void Dash(Vector2 direction)
        {
            StartCoroutine(DashCoroutine(direction));
        }

        private IEnumerator DashCoroutine(Vector2 direction)
        {
            if (_inDash) yield break;

            var startTime = Time.time;
            
            _inDash = true;
            
            while (Time.time - startTime < dashTime)
            {
                transform.Translate(direction.normalized * (speed * 5 * Time.deltaTime));
                yield return null;
            }
            
            _inDash = false;
        }
    }
}