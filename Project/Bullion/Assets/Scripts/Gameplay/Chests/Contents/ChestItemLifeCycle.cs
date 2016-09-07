using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests.Contents
{
    public class ChestItemLifeCycle : MonoBehaviour
    {
        public delegate void CollectionCallback(Transform collectedBy);

        private Rigidbody _rigidBody;

        private bool _canBeCollected;

        private CollectionCallback _collectionCallback;

        public void InitializeComponents(CollectionCallback collectionCallback)
        {
            _rigidBody = GetComponent<Rigidbody>();

            _collectionCallback = collectionCallback;
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
                if (_collectionCallback != null)
                {
                    _collectionCallback(collision.transform);
                }

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