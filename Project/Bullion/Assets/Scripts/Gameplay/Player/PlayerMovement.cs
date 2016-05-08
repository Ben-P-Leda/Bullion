using UnityEngine;
using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour, IConfigurable
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private PlayerInput _input;
        private Terrain _terrain;

        private float _seaEntryHeight;
        private float _swimHeight;
        private float _wadeHeightRange;

        public CharacterConfiguration Configuration { private get; set; }

        private void Start()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _input = GetComponent<PlayerInput>();
            _terrain = Terrain.activeTerrain;

            _seaEntryHeight = GetComponent<CapsuleCollider>().height * Sea_Entry_Height_Modifier;
            _swimHeight = GetComponent<CapsuleCollider>().height * Swim_Height_Modifier;
            _wadeHeightRange = _seaEntryHeight - _swimHeight;
        }

        private void Update()
        {
            float speed = GetMovementSpeed();
            Vector3 inputVelocity = new Vector3(_input.Horizontal, 0.0f, _input.Vertical).normalized * speed;

            if (inputVelocity != Vector3.zero)
            {
                _rigidBody.velocity = new Vector3(inputVelocity.x, _rigidBody.velocity.y, inputVelocity.z);
                _transform.LookAt(_transform.position + inputVelocity);
            }

            float groundHeight = _terrain.SampleHeight(_transform.position);
            _transform.position = new Vector3(_transform.position.x, Mathf.Max(_transform.position.y, groundHeight), _transform.position.z);
        }


        private float GetMovementSpeed()
        {
            float speed = Configuration.MovementSpeed;

            if (_transform.position.y < _seaEntryHeight)
            {
                float depthOffset = Mathf.Clamp((_seaEntryHeight - _transform.position.y) / _wadeHeightRange, 0.0f, 1.0f);
                speed *=  (1.0f - (Swim_Speed_Modifier * depthOffset));
            }

            return speed;
        }

        private const float Sea_Entry_Height_Modifier = 0.5f;
        private const float Swim_Height_Modifier = 0.05f;
        private const float Swim_Speed_Modifier = 0.4f;
    }
}