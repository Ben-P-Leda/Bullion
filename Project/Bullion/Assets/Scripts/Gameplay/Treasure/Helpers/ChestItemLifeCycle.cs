using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestItemLifeCycle : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private bool _canBeCollected;

        public string LaunchTriggerEvent { private get; set; }
        public float EventValue { private get; set; }

        public virtual void InitializeComponents()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
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
                EventDispatcher.FireEvent(_transform, collision.transform, LaunchTriggerEvent, EventValue);
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