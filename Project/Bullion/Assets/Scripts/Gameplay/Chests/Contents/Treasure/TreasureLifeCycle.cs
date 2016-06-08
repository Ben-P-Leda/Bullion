using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests.Contents.Treasure
{
    public class TreasureLifeCycle : ChestItemLifeCycle
    {
        private Transform _transform;

        public float Value;

        public void InitializeComponents()
        {
            _transform = transform;

            base.InitializeComponents(HandleCollection);
        }

        private void HandleCollection(Transform collectedBy)
        {
            EventDispatcher.FireEvent(_transform, collectedBy, EventMessage.Collect_Treasure, Value);
        }
    }
}