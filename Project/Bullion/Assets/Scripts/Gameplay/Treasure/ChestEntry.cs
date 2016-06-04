using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestEntry : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private BoxCollider _topCollider;
        private ChestCollisions _collisionController;
        private GameObject _groundCollider;
        private float _yPositionAtLaunch;

        private bool _inPlay;

        public float SecondsToLaunch { private get; set; }
        public Vector3 StartPosition { set { _transform.position = value; _yPositionAtLaunch = value.y; }  }
        public float HitPoints { set { _collisionController.HitPoints = value; } }

        public void InitializeComponents()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _topCollider = GetComponent<BoxCollider>();
            _collisionController = GetComponent<ChestCollisions>();
            _groundCollider = _transform.FindChild("Ground Collider").gameObject;
        }

        private void OnEnable()
        {
            _rigidBody.constraints =
                RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
            _rigidBody.useGravity = false;
            _topCollider.enabled = false;
            _groundCollider.SetActive(false);

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
            if ((!_groundCollider.activeInHierarchy) && (_transform.position.y > _yPositionAtLaunch + Activation_Height_Offset))
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
        private const float Activation_Height_Offset = 1.0f;
        private const float Ground_Lock_Velocity = 0.01f;
    }
}