using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player.ChestItemCollection
{
    public class PlayerChestItemCollectionDetector
    {
        public delegate void CollectionEventCallback(float itemParameter);

        private Transform _bodyTransform;
        private Transform _rushColliderTransform;
        private string _collectionEventMessage;
        private CollectionEventCallback _collectionEventCallback;

        public PlayerChestItemCollectionDetector(Transform bodyTransform, Transform rushColliderTransform, string collectionEventMessage,
            CollectionEventCallback collectionEventCallback)
        {
            _bodyTransform = bodyTransform;
            _rushColliderTransform = rushColliderTransform;
            _collectionEventMessage = collectionEventMessage;
            _collectionEventCallback = collectionEventCallback;
        }

        public void WireUpEventHandlers()
        {
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        public void UnhookEventHandlers()
        {
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((message == _collectionEventMessage) && (_collectionEventCallback != null) && (ItemCollectedByMe(target)))
            {
                _collectionEventCallback(value);
            }
        }

        private bool ItemCollectedByMe(Transform collectedBy)
        {
            if (collectedBy == _bodyTransform) { return true; }
            if (collectedBy == _rushColliderTransform) { return true; }

            return false;
        }
    }
}