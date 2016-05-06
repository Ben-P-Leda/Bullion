using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private PlayerInput _input;
        private Terrain _terrain;

        private void Start()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _input = GetComponent<PlayerInput>();
            _terrain = Terrain.activeTerrain;
        }

        private void Update()
        {
            Vector3 inputVelocity = new Vector3(_input.Horizontal, 0.0f, _input.Vertical).normalized * Movement_Speed;

            if (inputVelocity != Vector3.zero)
            {
                _rigidBody.velocity = new Vector3(inputVelocity.x, _rigidBody.velocity.y, inputVelocity.z);
                _transform.LookAt(_transform.position + inputVelocity);
            }

            float groundHeight = _terrain.SampleHeight(_transform.position);
            _transform.position = new Vector3(_transform.position.x, Mathf.Max(_transform.position.y, groundHeight), _transform.position.z);
        }

        private const float Movement_Speed = 5.0f;
    }
}