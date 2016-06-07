using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player.ChestItemCollection
{
    public class PlayerTreasureCount : MonoBehaviour
    {
        private Transform _transform;
        private PlayerChestItemCollectionDetector _collectionDetector;
        private float _treasureValue;

        private void Start()
        {
            _treasureValue = 0.0f;
        }

        private void OnEnable()
        {
            if (_collectionDetector == null)
            {
                _transform = transform;
                _collectionDetector = new PlayerChestItemCollectionDetector(
                    _transform, _transform.FindChild("Rush Collider"), EventMessage.Collect_Treasure, HandleTreasureCollection);
            }

            _collectionDetector.WireUpEventHandlers();
        }

        private void OnDisable()
        {
            _collectionDetector.UnhookEventHandlers();
        }

        private void HandleTreasureCollection(float treasureValue)
        {
            _treasureValue += treasureValue;
            EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Treasure, _treasureValue);
        }
    }
}
