using UnityEngine;

namespace Server
{
    public class Player
    {
        private GameObject _gameObject;
        private float _previousTime;
        private float _speed;

        public bool InDash = false; 
        public float DashTime = 0.3f;

        public Player(GameObject gameObject, float speed)
        {
            _gameObject = gameObject;
            _speed = speed;
        }

        public GameObject PlayerGameObject
        {
            get
            {
                return _gameObject;
            }
        }

        public float PreviousTime
        {
            get => _previousTime;

            set
            {
                _previousTime = value;
                if (_previousTime < PreviousTime)
                    _previousTime = PreviousTime;
            } 
        }

        public float Speed
        {
            get => _speed;

            set
            {
                _speed = value;
                if (Speed >= 0)
                    _speed = Speed;
            }
        }
    }
}