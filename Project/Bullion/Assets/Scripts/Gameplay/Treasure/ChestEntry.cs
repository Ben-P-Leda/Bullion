using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestEntry : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private BoxCollider _topCollider;
        private GameObject _groundCollider;

        private bool _inPlay;

        public float SecondsToLaunch { private get; set; }
        public Vector3 StartPosition { private get; set; }

        private void Start()
        {
            _transform = transform;
            _transform.position = StartPosition;
            _rigidBody = _transform.GetComponent<Rigidbody>();
            _topCollider = _transform.GetComponent<BoxCollider>();
            _groundCollider = _transform.FindChild("Ground Collider").gameObject;

            _inPlay = false;
        }

        private void Update()
        {
            if (!_inPlay)
            {
                if (SecondsToLaunch > 0.0f)
                {
                    UpdateLaunchTimer();
                }
                else
                {
                    UpdateEntryPhysicsState();
                }
            }
        }

        private void UpdateLaunchTimer()
        {
            SecondsToLaunch -= Time.deltaTime;
            if (SecondsToLaunch <= 0.0f)
            {
                _rigidBody.AddForce(Vector3.up * Launch_Speed, ForceMode.VelocityChange);
            }
        }

        private void UpdateEntryPhysicsState()
        {
            if ((!_groundCollider.activeInHierarchy) && (_transform.position.y > StartPosition.y + Activation_Height_Offset))
            {
                _topCollider.enabled = true;
                _groundCollider.SetActive(true);
                _rigidBody.velocity = Vector3.up;
                _rigidBody.useGravity = true;
            }

            if ((_rigidBody.useGravity) && (_rigidBody.velocity.magnitude <= Ground_Lock_Velocity))
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                _inPlay = true;
            }
        }

        private const float Launch_Speed = 20.0f;
        private const float Activation_Height_Offset = 1.75f;
        private const float Ground_Lock_Velocity = 0.01f;
    }
}