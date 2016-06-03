using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class TreasureLifeCycle : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private bool _canBeCollected;

        public float Value;

        private void Start()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();

            SetForLaunch();
        }

        private void OnEnable()
        {
            if (_transform != null)
            {
                SetForLaunch();
            }
        }

        private void SetForLaunch()
        {
            _canBeCollected = false;

            Vector2 travel = Random.insideUnitCircle * 250.0f;
            _rigidBody.AddForce(new Vector3(travel.x, 500.0f, travel.y));
        }

        private void Update()
        {
            if ((!_canBeCollected) && (_rigidBody.velocity.y < 0.0f))
            {
                _canBeCollected = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if ((_canBeCollected) && (CollidingObjectCanCollectTreasure(collision.gameObject)))
            {
                EventDispatcher.FireEvent(_transform, collision.transform, EventMessage.Collect_Treasure, Value);
                gameObject.SetActive(false);
            }
        }

        private bool CollidingObjectCanCollectTreasure(GameObject collidingObject)
        {
            if (collidingObject.tag == Constants.Player_Tag) { return true; }
            if (collidingObject.tag == Constants.Rush_Collider_Tag) { return true; }

            return false;
        }
    }
}