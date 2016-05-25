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
        private float _groundHeight;

        // TODO: Should be a properties with private getter
        public float SecondsToLaunch;

        public Vector3 StartPosition
        {
            set
            {
                _transform.position = new Vector3(value.x, _groundHeight + Hidden_Chest_Vertical_Offset, value.z);
            }
        }

        private void Start()
        {
            _transform = transform;
            _rigidBody = _transform.GetComponent<Rigidbody>();
            _topCollider = _transform.GetComponent<BoxCollider>();
            _groundCollider = _transform.FindChild("Ground Collider").gameObject;

            _inPlay = false;
            _groundHeight = Terrain.activeTerrain.SampleHeight(_transform.position);

            // Should be set by factory
            StartPosition = new Vector3(22, 0, 20);
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
            if ((!_groundCollider.activeInHierarchy) && (_transform.position.y > _groundHeight + Activation_Height_Offset))
            {
                _topCollider.enabled = true;
                _groundCollider.SetActive(true);
                _rigidBody.velocity = Vector3.up;
                _rigidBody.useGravity = true;
            }

            if ((_transform.position.y - Ground_Proximity_Offset <= _groundHeight) && (_rigidBody.useGravity)
                && (_rigidBody.velocity.magnitude <= Ground_Lock_Velocity))
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                _inPlay = true;
            }
        }

        //private void UpdateForEntrySequence()
        //{
        //    if (SecondsToLaunch > 0.0f)
        //    {
        //        SecondsToLaunch -= Time.deltaTime;
        //        if (SecondsToLaunch <= 0.0f)
        //        {
        //            _rigidBody.AddForce(Vector3.up * Launch_Speed, ForceMode.VelocityChange);
        //        }
        //    }

        //    if (SecondsToLaunch <= 0.0f)
        //    {
        //        if ((!_groundCollider.activeInHierarchy) && (_transform.position.y > _groundHeight + Activation_Height_Offset))
        //        {
        //            _topCollider.enabled = true;
        //            _groundCollider.SetActive(true);
        //            _rigidBody.velocity = Vector3.up;
        //            _rigidBody.useGravity = true;
        //        }

        //        if ((_transform.position.y - Ground_Proximity_Offset <= _groundHeight) && (_rigidBody.useGravity)
        //            && (_rigidBody.velocity.magnitude <= Ground_Lock_Velocity))
        //        {
        //            _rigidBody.velocity = Vector3.zero;
        //            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        //            _inPlay = true;
        //        }
        //    }
        //}

        private const float Hidden_Chest_Vertical_Offset = -0.75f;
        private const float Launch_Speed = 20.0f;
        private const float Activation_Height_Offset = 0.75f;
        private const float Ground_Proximity_Offset = 1.0f;
        private const float Ground_Lock_Velocity = 0.01f;
    }
}